using MultiThreading.Task3.MatrixMultiplier.Matrices;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public interface IMatricesMultiplier
    {
        void Multiply(IMatrix m1, IMatrix m2, IMatrix result);
    }
}
