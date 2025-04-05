import Calculator from './components/Calculator';

export default function Home() {
  return (
    <main className="min-h-screen bg-gray-100 py-12">
      <div className="container mx-auto px-4">
        <h1 className="mb-8 text-center text-4xl font-bold text-gray-900">
          Probability Calculator
        </h1>
        <Calculator />
      </div>
    </main>
  );
}
