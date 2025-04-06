'use client';
import { useMemo } from 'react';
import { useForm, useWatch } from 'react-hook-form';

import { useProbability } from '@/client/hooks/useProbability';

type Inputs = {
  probabilityA: string;
  probabilityB: string;
  calculationType: string;
};

export default function Calculator() {
  const {
    register,
    control,
    formState: { errors, isValid },
  } = useForm<Inputs>({
    mode: 'onChange',
    reValidateMode: 'onChange',
    defaultValues: {
      probabilityA: '0',
      probabilityB: '0',
      calculationType: '0',
    },
  });

  const probabilityA = useWatch({
    name: 'probabilityA',
    control,
    exact: false,
  });
  const probabilityB = useWatch({
    name: 'probabilityB',
    control,
    exact: false,
  });
  const calculationType = useWatch({
    name: 'calculationType',
    control,
    exact: false,
  });

  const calculationParams = useMemo(
    () => ({
      probabilityA: parseFloat(probabilityA),
      probabilityB: parseFloat(probabilityB),
      calculationType: parseInt(calculationType),
      isValid: isValid,
    }),
    [probabilityA, probabilityB, calculationType, isValid]
  );

  const { probability } = useProbability(calculationParams);

  return (
    <div className="mx-auto max-w-md rounded-lg bg-white p-6 shadow-lg">
      <form className="space-y-4">
        <div>
          <label
            htmlFor="probabilityA"
            className="block text-sm font-medium text-gray-700"
          >
            Probability A
          </label>
          <input
            {...register('probabilityA', {
              required: 'Probability A is required',
              min: { value: 0, message: 'Probability must be between 0 and 1' },
              max: { value: 1, message: 'Probability must be between 0 and 1' },
            })}
            data-testid="probabilityA"
            type="number"
            step="0.01"
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:outline-blue-500"
          />
          {errors.probabilityA && (
            <div
              data-testid="probabilityA-error"
              className="mt-4 rounded border border-red-400 bg-red-100 p-3 text-red-700"
            >
              {errors.probabilityA?.message}
            </div>
          )}
        </div>
        <div>
          <label
            htmlFor="probabilityB"
            className="block text-sm font-medium text-gray-700"
          >
            Probability B
          </label>
          <input
            {...register('probabilityB', {
              required: 'Probability B is required',
              min: { value: 0, message: 'Probability must be between 0 and 1' },
              max: { value: 1, message: 'Probability must be between 0 and 1' },
            })}
            data-testid="probabilityB"
            type="number"
            step="0.01"
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:outline-blue-500"
          />
          {errors.probabilityB && (
            <div
              data-testid="probabilityB-error"
              className="mt-4 rounded border border-red-400 bg-red-100 p-3 text-red-700"
            >
              {errors.probabilityB?.message}
            </div>
          )}
        </div>
        <div>
          <label
            htmlFor="calculationType"
            className="block text-sm font-medium text-gray-700"
          >
            Calculation Type
          </label>
          <select
            {...register('calculationType', {
              required: 'Calculation Type is required',
            })}
            data-testid="calculationType"
            className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:outline-blue-500"
          >
            <option value={'0'}>Combined With</option>
            <option value={'1'}>Either</option>
          </select>
        </div>
      </form>
      {!!probability && (
        <div
          data-testid="probability"
          className="mt-4 rounded border border-green-400 bg-green-100 p-3 text-green-700"
        >
          Probability: {probability?.toFixed(4)}
          <span> ({((probability ?? 0) * 100).toFixed(2)}%)</span>
        </div>
      )}
    </div>
  );
}
