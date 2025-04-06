import axios from 'axios';

import { getSettings } from '../../server/utils';

export type ProbabilityResponseWithStatus = {
  data: number | null;
  status: number;
};

export const probabilityFetcher = async (params: {
  probabilityA: number;
  probabilityB: number;
  calculationType: number;
}): Promise<ProbabilityResponseWithStatus> => {
  try {
    const { apiBaseUrl } = await getSettings();

    const response = await axios.get<number>(
      `${apiBaseUrl}calculate/${params.calculationType}/${params.probabilityA}/${params.probabilityB}`
    );
    console.log(response);
    return { data: response.data, status: 200 };
  } catch {
    return { data: null, status: 500 };
  }
};
