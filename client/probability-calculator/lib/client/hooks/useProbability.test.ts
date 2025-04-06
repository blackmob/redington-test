import { renderHook, waitFor } from '@testing-library/react';
import { http, HttpResponse } from 'msw';
import { setupServer } from 'msw/node';
import {
  afterAll,
  afterEach,
  beforeAll,
  beforeEach,
  describe,
  expect,
  it,
  vi,
} from 'vitest';

import { useProbability } from '@/lib/client/hooks/useProbability';

const handlers = [
  http.get('calculate/*/*/*', () => {
    return HttpResponse.text('0.5');
  }),
];

const server = setupServer(...handlers);

describe('useProbability', () => {
  beforeAll(() => server.listen());
  afterAll(() => server.close());
  afterEach(() => server.resetHandlers());

  beforeEach(() => {
    // Clear all mocks before each test
    vi.clearAllMocks();
  });

  it('should return undefined when isValid is false', () => {
    const { result } = renderHook(() =>
      useProbability({
        isValid: false,
        probabilityB: 0.5,
        probabilityA: 0.5,
        calculationType: 0,
      })
    );
    expect(result.current.probability).toBeUndefined();
  });

  it('should return data when isValid and parameters are sent', async () => {
    const { result } = renderHook(() =>
      useProbability({
        isValid: true,
        probabilityB: 0.5,
        probabilityA: 0.5,
        calculationType: 0,
      })
    );

    await waitFor(() => expect(result.current.probability).toBe(0.5));
    expect(result.current.probability).toBe(0.5);
  });
});
