import { render, screen, waitFor } from '@testing-library/react';
import { userEvent } from '@testing-library/user-event';
import { beforeEach, describe, expect, it, vi } from 'vitest';

import Calculator from '@/app/components/Calculator';

vi.mock('@/lib/client/hooks/useProbability', () => ({
  useProbability: vi.fn().mockImplementation((params) => {
    if (!params.isValid) return { probability: null };
    if (params.calculationType === 0) {
      // Combined With
      return { probability: params.probabilityA * params.probabilityB };
    } else {
      // Either
      return {
        probability:
          params.probabilityA +
          params.probabilityB -
          params.probabilityA * params.probabilityB,
      };
    }
  }),
}));

describe('Calculator Component', () => {
  beforeEach(() => {
    // Clear all mocks before each test
    vi.clearAllMocks();
  });

  it('renders the calculator form with all inputs', () => {
    render(<Calculator />);
    // Check for input fields
    expect(screen.getByTestId('probabilityA')).toBeInTheDocument();
    expect(screen.getByTestId('probabilityB')).toBeInTheDocument();
    expect(screen.getByTestId('calculationType')).toBeInTheDocument();
  });

  it('validates probability inputs to be between 0 and 1', async () => {
    render(<Calculator />);

    const probAInput = screen.getByTestId('probabilityA');
    const probBInput = screen.getByTestId('probabilityB');

    // Test invalid values

    await userEvent.clear(probAInput);
    await userEvent.type(probAInput, '1.5');
    expect(screen.getByTestId('probabilityA-error')).toBeInTheDocument();

    await userEvent.clear(probBInput);
    await userEvent.type(probBInput, '-0.5');
    expect(screen.getByTestId('probabilityB-error')).toBeInTheDocument();
  });

  it('calculates combined probability correctly', async () => {
    render(<Calculator />);

    const probAInput = screen.getByTestId('probabilityA');
    const probBInput = screen.getByTestId('probabilityB');
    const select = screen.getByTestId('calculationType');

    await userEvent.type(probAInput, '0.5');
    await userEvent.type(probBInput, '0.3');
    await userEvent.selectOptions(select, '0');

    // Wait for the result to appear
    await waitFor(() => {
      expect(screen.getByTestId('probability')).toBeInTheDocument();
      expect(screen.getByText(/0.1500/)).toBeInTheDocument(); // 0.5 * 0.3 = 0.15
    });
  });

  it('calculates either probability correctly', async () => {
    render(<Calculator />);

    const probAInput = screen.getByTestId('probabilityA');
    const probBInput = screen.getByTestId('probabilityB');
    const select = screen.getByTestId('calculationType');

    // Set values for either probability
    await userEvent.type(probAInput, '0.5');
    await userEvent.type(probBInput, '0.3');

    // Select "Either"
    await userEvent.selectOptions(select, '1');

    // Wait for the result to appear
    await waitFor(() => {
      expect(screen.getByText(/probability:/i)).toBeInTheDocument();
      expect(screen.getByText(/0.6500/)).toBeInTheDocument(); // 0.5 + 0.3 - (0.5 * 0.3) = 0.65
    });
  });

  it('shows no result when inputs are invalid', async () => {
    render(<Calculator />);

    const probAInput = screen.getByTestId('probabilityA');

    // Enter invalid value
    await userEvent.clear(probAInput);
    await userEvent.type(probAInput, '1.5');

    // Check that no result is shown
    expect(screen.queryByTestId('probability')).not.toBeInTheDocument();
  });

  it('handles empty inputs correctly', async () => {
    render(<Calculator />);

    const probAInput = screen.getByTestId('probabilityA');
    const probBInput = screen.getByTestId('probabilityB');

    // Clear inputs
    await userEvent.clear(probAInput);
    await userEvent.clear(probBInput);

    // Check for required field errors
    expect(screen.getByText('Probability A is required')).toBeInTheDocument();
    expect(screen.getByText('Probability B is required')).toBeInTheDocument();
  });
});
