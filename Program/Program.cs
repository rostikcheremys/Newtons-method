using System;

namespace Program
{
    abstract class Program 
    {
        public static void Main()
        {
            double x1 = 0;
            double x2 = 0;
            double epsilon = 0.001;

            Newton(x1, x2, epsilon);
        }
        
        // Функції системи рівнянь
        private static double Function1(double x1, double x2) 
        {
            return Math.Sin(x1) - Math.Pow(x2, 2);
        }

        private static double Function2(double x1, double x2) 
        {
            return Math.Pow(Math.Tan(x1), 2) - x2;
        }

        // Матриця Якобі
        private static double[,] ComputeJacobian(double x1, double x2) 
        {
            double[,] jacobian = new double[2, 2];

            // Похідні по x1
            jacobian[0, 0] = Math.Cos(x1);
            jacobian[1, 0] = 2 * Math.Tan(x1) * (1 / Math.Pow(Math.Cos(x1), 2));

            // Похідні по x2
            jacobian[0, 1] = -2 * x2;
            jacobian[1, 1] = -1;

            return jacobian;
        }
        
        // Метод Ньютона
        private static void Newton(double x1, double x2, double epsilon) 
        {
            int iterations = 0;
            
            double deltaX1, deltaX2;
            
            if (!СheckСonvergence(x1, x2))
            {
                Console.WriteLine("Початкові дані не збіжні.");
                return;
            }
            
            do 
            {
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
                
            } while (Math.Abs(deltaX1) > epsilon || Math.Abs(deltaX2) > epsilon);
            
            Console.WriteLine("Коренi: x1 = " + x1 + ", x2 = " + x2);
            Console.WriteLine("Кiлькiсть iтерацiй: " + iterations);
        }
        
        private static bool СheckСonvergence(double x1, double x2)
        {
            double[,] jacobian = ComputeJacobian(x1, x2);

            double convergence1 = Math.Abs(jacobian[0, 0]) + Math.Abs(jacobian[0, 1]);
            double convergence2 = Math.Abs(jacobian[1, 0]) + Math.Abs(jacobian[1, 1]);
            
            return convergence1 <= 1 & convergence2 <= 1;
        }
    }
}
