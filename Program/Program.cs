using System;

class NewtonMethod
{
    // Функция, представляющая систему уравнений
    static double[] F(double[] x)
    {
        double[] result = new double[2];
        result[0] = x[0] * x[0] - 2 * x[0] - x[1] + 0.5;
        result[1] = x[0] * x[0] + 4 * x[1] * x[1] - 4;
        return result;
    }

    // Матрица Якоби для системы уравнений
    static double[,] Jacobian(double[] x)
    {
        double[,] jacobian = new double[2, 2];
        jacobian[0, 0] = 2 * x[0] - 2;
        jacobian[0, 1] = -1;
        jacobian[1, 0] = 2 * x[0];
        jacobian[1, 1] = 8 * x[1];
        return jacobian;
    }

    // Обратная матрица
    static double[,] InvertMatrix(double[,] matrix)
    {
        double det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

        if (Math.Abs(det) < 0.0001)
        {
            throw new Exception("Matrix is singular and cannot be inverted.");
        }

        double invDet = 1.0 / det;

        double[,] inverse = new double[2, 2];
        inverse[0, 0] = matrix[1, 1] * invDet;
        inverse[0, 1] = -matrix[0, 1] * invDet;
        inverse[1, 0] = -matrix[1, 0] * invDet;
        inverse[1, 1] = matrix[0, 0] * invDet;

        return inverse;
    }

    // Умножение матрицы на вектор
    static double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
    {
        double[] result = new double[2];

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                result[i] += matrix[i, j] * vector[j];
            }
        }

        return result;
    }

    // Вычитание векторов
    static double[] SubtractVectors(double[] vector1, double[] vector2)
    {
        double[] result = new double[2];

        for (int i = 0; i < 2; i++)
        {
            result[i] = vector1[i] - vector2[i];
        }

        return result;
    }

    // Норма вектора
    static double Norm(double[] vector)
    {
        double sum = 0.0;

        for (int i = 0; i < 2; i++)
        {
            sum += vector[i] * vector[i];
        }

        return Math.Sqrt(sum);
    }

    // Метод Ньютона для системы уравнений
    static double[] NewtonMethodSolver(Func<double[], double[]> F, Func<double[], double[,]> Jacobian, double[] initialGuess, double tolerance = 1e-6, int maxIterations = 100)
    {
        double[] x = initialGuess;
        int iteration = 0;

        while (iteration < maxIterations)
        {
            double[] fValues = F(x);
            double[,] jacobianMatrix = Jacobian(x);

            double[,] inverseJacobian = InvertMatrix(jacobianMatrix);

            double[] deltaX = MultiplyMatrixVector(inverseJacobian, fValues);

            x = SubtractVectors(x, deltaX);

            double norm = Norm(deltaX);

            if (norm < tolerance)
                break;

            iteration++;
        }

        return x;
    }

    static void Main()
    {
        // Начальное приближение
        double[] initialGuess = { 1.0, 1.0 };

        // Решение системы уравнений методом Ньютона
        double[] result = NewtonMethodSolver(F, Jacobian, initialGuess);

        // Вывод результатов
        Console.WriteLine("Solution:");
        Console.WriteLine($"x = {result[0]}, y = {result[1]}");
    }
}
