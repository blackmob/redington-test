import useSWR, { SWRConfiguration } from 'swr';

import { probabilityFetcher, ProbabilityResponseWithStatus } from './fetchers';

type CalculateProbability = {
  probabilityA: number;
  probabilityB: number;
  calculationType: number;
  isValid: boolean;
} & SWRConfiguration;

const useProbability = (config: CalculateProbability) => {
  const { probabilityA, probabilityB, calculationType, isValid, ...swrConfig } =
    config;
  const { data, error, isValidating } = useSWR<ProbabilityResponseWithStatus>(
    isValid ? { probabilityA, probabilityB, calculationType } : null,
    probabilityFetcher,
    swrConfig
  );
  return {
    probability: data?.data,
    status: data?.status,
    isLoading: isValidating,
    isError: !!error,
  };
};

export { useProbability };
