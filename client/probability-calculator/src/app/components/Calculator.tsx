'use client';

import { useState } from 'react';
import axios from 'axios';

export default function Calculator() {
  const [probabilityA, setProbabilityA] = useState('');
  const [probabilityB, setProbabilityB] = useState('');
  const [result, setResult] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);

  const validateProbability = (value: string): boolean => {
    const num = parseFloat(value);
    return !isNaN(num) && num >= 0 && num <= 1;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setResult(null);

    if (!validateProbability(probabilityA) || !validateProbability(probabilityB)) {
      setError('Both probabilities must be numbers between 0 and 1');
      return;
    }

    try {
      const response = await axios.post('/api/calculate', {
        probabilityA: parseFloat(probabilityA),
        probabilityB: parseFloat(probabilityB)
      });
      setResult(response.data.result);
    } catch (err: any) {
      setError(err.response?.data || 'Failed to calculate combined probability. Please try again.');
      console.error('Error:', err);
    }
  };

  return (
    <div className="max-w-md mx-auto p-6 bg-white rounded-lg shadow-lg">
      <h2 className="text-2xl font-bold mb-6 text-gray-800">Probability Calculator</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="probabilityA" className="block text-sm font-medium text-gray-700">
            Probability A
          </label>
          <input
            type="number"
            id="probabilityA"
            step="0.01"
            min="0"
            max="1"
            value={probabilityA}
            onChange={(e) => setProbabilityA(e.target.value)}
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:ring-blue-500"
            required
          />
        </div>
        <div>
          <label htmlFor="probabilityB" className="block text-sm font-medium text-gray-700">
            Probability B
          </label>
          <input
            type="number"
            id="probabilityB"
            step="0.01"
            min="0"
            max="1"
            value={probabilityB}
            onChange={(e) => setProbabilityB(e.target.value)}
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:ring-blue-500"
            required
          />
        </div>
        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          Calculate Combined Probability
        </button>
      </form>
      {error && (
        <div className="mt-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}
      {result !== null && (
        <div className="mt-4 p-3 bg-green-100 border border-green-400 text-green-700 rounded">
          Combined Probability: {result.toFixed(4)}
        </div>
      )}
    </div>
  );
} 