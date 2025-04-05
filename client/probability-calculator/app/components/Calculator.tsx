'use client';

import axios from 'axios';
import { useState } from 'react';

export default function Calculator() {
  const [probabilityA, setProbabilityA] = useState('');
  const [probabilityB, setProbabilityB] = useState('');
  const [result, setResult] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [calculationType, setCalculationType] = useState('0');

  const validateProbability = (value: string): boolean => {
    const num = parseFloat(value);
    return !isNaN(num) && num >= 0 && num <= 1;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setResult(null);

    if (
      !validateProbability(probabilityA) ||
      !validateProbability(probabilityB)
    ) {
      setError('Both probabilities must be numbers between 0 and 1');
      return;
    }

    try {
      const response = await axios.post('http://localhost:5004/calculate ', {
        probabilityA: parseFloat(probabilityA),
        probabilityB: parseFloat(probabilityB),
        calculationType: parseInt(calculationType),
      });
      setResult(response.data);
    } catch {
      setError('Failed to calculate combined probability. Please try again.');
    }
  };

  return (
    <div className="mx-auto max-w-md rounded-lg bg-white p-6 shadow-lg">
      <h2 className="mb-6 text-2xl font-bold text-gray-800">
        Probability Calculator
      </h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label
            htmlFor="probabilityA"
            className="block text-sm font-medium text-gray-700"
          >
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
          <label
            htmlFor="probabilityB"
            className="block text-sm font-medium text-gray-700"
          >
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

        <div>
          <label
            htmlFor="calculationType"
            className="block text-sm font-medium text-gray-700"
          >
            Calculation Type
          </label>
          <select
            id="calculationType"
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:ring-blue-500"
            onChange={(e) => setCalculationType(e.target.value)}
            required
          >
            <option value={'0'}>Combined With</option>
            <option value={'1'}>Either</option>
          </select>
        </div>

        <button
          type="submit"
          className="w-full rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:outline-none"
        >
          Calculate Probability
        </button>
      </form>
      {error && (
        <div className="mt-4 rounded border border-red-400 bg-red-100 p-3 text-red-700">
          {error}
        </div>
      )}
      {result !== null && (
        <div className="mt-4 rounded border border-green-400 bg-green-100 p-3 text-green-700">
          Probability: {result.toFixed(4)}
        </div>
      )}
    </div>
  );
}
