using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.functions
{
   public static class SwapGeneric<T>
    {
        public static void  Swap(List<T> l,int index1 , int index2) {
            T temp;
            temp = l[index1];
            l[index1] = l[index2];
            l[index2] = temp;
        }
    }
}
