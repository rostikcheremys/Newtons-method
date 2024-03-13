using System;

class NewtonMethod {
    // Функції системи рівнянь
    static double Function1(double x1, double x2) 
    {
        return Math.Sin(x1) - Math.Pow(x2, 2);
    }

    static double Function2(double x1, double x2) 
    {
        return Math.Pow(Math.Tan(x1), 2) - x2;
    }

    // Функція для обчислення похідних
    static double[,] ComputeJacobian(double x1, double x2) 
    {
        double[,] jacobian = new double[2, 2];

        // Похідні по x1
        jacobian[0, 0] = Math.Cos(x1);
        jacobian[1, 0] = 2 * Math.Tan(x1) * Math.Pow(Math.Tan(x1), 2) + 1;

        // Похідні по x2
        jacobian[0, 1] = -2 * x2;
        jacobian[1, 1] = -1;

        return jacobian;
    }

    // Метод Ньютона
    static void Newton(double x1, double x2, double epsilon = 0.0001, int maxIterations = 100) {
        
        int iterations = 0;
        double deltaX1, deltaX2;

        do {
            // Обчислення матриці Якобі
            double[,] jacobian = ComputeJacobian(x1, x2);

            // Обчислення оберненої матриці Якобі
            double det = jacobian[0, 0] * jacobian[1, 1] - jacobian[0, 1] * jacobian[1, 0];
            
            double invDet = 1 / det;
            
            double[,] inverseJacobian = 
            {
                { jacobian[1, 1] * invDet, -jacobian[0, 1] * invDet },
                { -jacobian[1, 0] * invDet, jacobian[0, 0] * invDet }
            };

            // Обчислення функцій системи рівнянь
            double[] functions = { Function1(x1, x2), Function2(x1, x2) };

            // Обчислення нового наближення
            deltaX1 = inverseJacobian[0, 0] * functions[0] + inverseJacobian[0, 1] * functions[1];
            deltaX2 = inverseJacobian[1, 0] * functions[0] + inverseJacobian[1, 1] * functions[1];

            x1 -= deltaX1;
            x2 -= deltaX2;

            iterations++;
        } while ((Math.Abs(deltaX1) > epsilon || Math.Abs(deltaX2) > epsilon) && iterations < maxIterations);

        Console.WriteLine("Корені: x1 = " + x1 + ", x2 = " + x2);
        Console.WriteLine("Кількість ітерацій: " + iterations);
    }

    static void Main() 
    {
        double initialGuessX1 = 0.74; 
        double initialGuessX2 = 0.82; 

        Newton(initialGuessX1, initialGuessX2);
    }
}
