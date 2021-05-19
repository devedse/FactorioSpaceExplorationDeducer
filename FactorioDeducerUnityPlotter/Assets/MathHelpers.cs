using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MathHelpers
{
    public static bool IsCloseToZero(decimal a)
    {
        return IsCloseBy(a, 0);
    }

    public static bool IsCloseBy(decimal a, decimal b)
    {
        var diff = Math.Abs(a - b);

        if (diff < 0.00001m)
        {
            return true;
        }
        return false;

    }
}
