import axios from 'axios';

import { getSettings } from '../../server/utils';

type ProbabilityResponseWithStatus = {
  data: ProbabilityResponse | null;
  status: number;
};

type ProbabilityResponse = { result: number };

export const probabilityFetcher = async (params: {
  probabilityA: number;
  probabilityB: number;
  calculationType: number;
}): Promise<ProbabilityResponseWithStatus> => {
  try {
    const { apiBaseUrl } = await getSettings();

    const response = await axios.post<ProbabilityResponse>(apiBaseUrl, {
      probabilityA: params.probabilityA,
      probabilityB: params.probabilityB,
      calculationType: params.calculationType,
    });
    return { data: response.data, status: 200 };
  } catch {
    return { data: null, status: 500 };
  }
};
