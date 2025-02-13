using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Console.ReadKey();

            //конструктор метода
            GetInfo getInfo = new GetInfo();
            int[] SuplierArray = getInfo.Supplier();
            int[] CustomerArray = getInfo.Customers();
            int[,] TraficPlan = getInfo.DataPlans(SuplierArray, CustomerArray);



            //количество а(поставщик)
            int i = SuplierArray.Length;
            //количество б(заказчик)
            int j = CustomerArray.Length;


            //сумма всех поставок
            int SummaSuplier = 0;
            //сумма нужд заказчика
            int SummaCustomer = 0;

            for (int i2 = 0; i2 < i; i2++)
            { SummaSuplier = SummaSuplier + SuplierArray[i2]; }

            for (int i2 = 0; i2 < j; i2++)
            { SummaCustomer = SummaCustomer + CustomerArray[i2]; }

            if (SummaSuplier == SummaCustomer)
            {
                Console.WriteLine("Транспортная задача - закрытая");

                //ЗДЕСЬ ВЫБИРАТЬ МЕТОД
                int[,] OpPlan = MinElement(SuplierArray, CustomerArray, TraficPlan, i, j);
                TargeFunction(OpPlan, TraficPlan, i, j);

            }
            else
            {
                Console.WriteLine("Транспортная задача - открытая");
                Console.WriteLine("Пока-пока)");
            }

            /*bool CloseOrOpen = getInfo.OpenOrClose(SummaCustomer, SummaSuplier);*/

        }


        //северо-западного угла
        public static int[,] NorthWesternCornerFun(int[] SuplierArray, int[] CustomerArray)
        {
            Console.ReadKey();

            int i = 0; //первый индекс движения
            int j = 0; //второй индекс движения

            int[,] OporniyPlan = new int[SuplierArray.Length, CustomerArray.Length];

            int b = 0; //остаток
            while (i != SuplierArray.Length && j != CustomerArray.Length)
            {
                OporniyPlan[i, j] = Math.Min(SuplierArray[i], CustomerArray[j]);
                b = CustomerArray[j] - OporniyPlan[i, j];
                SuplierArray[i] = SuplierArray[i] - OporniyPlan[i, j];
                CustomerArray[j] = CustomerArray[j] - OporniyPlan[i, j];
                //если остаток ноль 
                if (b == 0)
                {
                    //заполняем другие поля по вертикали нулями 0
                    for (int i2 = i + 1; i2 < SuplierArray.Length; i2++)
                    {
                        OporniyPlan[i2, j] = 0;
                    }
                    //из поставщика вычла сколько отдала покупателю

                    j = j + 1;
                }
                else
                {
                    for (int i2 = j + 1; i2 < CustomerArray.Length; i2++)
                    {
                        OporniyPlan[i, i2] = 0;
                    }
                    i = i + 1;
                }
            }

            for (int i2 = 0; i2 < SuplierArray.Length; ++i2)
            {
                for (int j2 = 0; j2 < CustomerArray.Length; j2++)
                {
                    Console.Write("{0}\t", OporniyPlan[i2, j2]);
                }
                Console.WriteLine();
            }
            return OporniyPlan;
        }

        //Метод минимального элемента
        public static int[,] MinElement(int[] SuplierArray, int[] CustomerArray, int[,] Trafic, int q, int w)
        {
            //массив опорного плана
            int[,] OporniyPlan = new int[SuplierArray.Length, CustomerArray.Length];
            //заполняю всё -1
            for (int i = 0; i < OporniyPlan.GetLength(0); i++)
            {
                for (int j = 0; j < OporniyPlan.GetLength(1); j++)
                {
                    OporniyPlan[i, j] = -1;
                }
            }

            //Чтобы знать все тарифы и их индексы
            //лист содержащий 1-элемент 2-первый индекс 3-второй индекс
            List<(int value, int ii, int jj)> IndexMinElArray = new List<(int value, int ii, int jj)>();


            //Заполняю массив 
            for (int i = 0; i < q; i++)
            {
                for (int y = 0; y < w; y++)
                {
                    IndexMinElArray.Add((Trafic[i, y], i, y));
                }
            }

            //его сортировка от мин к макс
            IndexMinElArray.Sort((a, b) => a.value.CompareTo(b.value));


            //счётчик - на каком мы элементе, r не получается использовать
            int n = 0;
            int z = 0;//остаток
            for (int r = 0; r < q * w; r++)
            {
                if (OporniyPlan[IndexMinElArray[n].ii, IndexMinElArray[n].jj] != 0)
                {
                    //в опорном плане с координатами из IndexMinElArray[i][j] он находит минимальный элемент у склада и покупателя с теми же координатами
                    OporniyPlan[IndexMinElArray[n].ii, IndexMinElArray[n].jj] = Math.Min(SuplierArray[IndexMinElArray[n].ii], CustomerArray[IndexMinElArray[n].jj]);
                    //высчитывает остаток
                    z = CustomerArray[IndexMinElArray[n].jj] - OporniyPlan[IndexMinElArray[n].ii, IndexMinElArray[n].jj];
                    //Из склада вычитаю сколько записали в ячейку
                    SuplierArray[IndexMinElArray[n].ii] = SuplierArray[IndexMinElArray[n].ii] - OporniyPlan[IndexMinElArray[n].ii, IndexMinElArray[n].jj];
                    //также вычитаю из покупателя
                    CustomerArray[IndexMinElArray[n].jj] = CustomerArray[IndexMinElArray[n].jj] - OporniyPlan[IndexMinElArray[n].ii, IndexMinElArray[n].jj];

                    //если остаток ноль (Заполняю столбец нулями)
                    if (z == 0)
                    {
                        //заполняем другие поля по вертикали нулями 0
                        for (int i2 = 0; i2 < SuplierArray.Length; i2++)
                        {
                            //чтобы мой индекс заполнения не был равен тому где только что заполнина опорный план
                            if (i2 != IndexMinElArray[n].ii)
                            {
                                //заполняю нулём столбец только те ячейки где значение меньше нуля
                                if (OporniyPlan[i2, IndexMinElArray[n].jj] < 0 /*|| OporniyPlan[i2, IndexMinElArray[n].jj] == -1*/)
                                { OporniyPlan[i2, IndexMinElArray[n].jj] = 0; }
                            }
                        }
                    }
                    //иначе если остаток не ноль (Заполняю строку нулями)
                    else
                    {

                        for (int i2 = 0; i2 < CustomerArray.Length; i2++)
                        {
                            //чтобы мой индекс заполнения не был равен тому где только что заполнила опорный план
                            if (i2 != IndexMinElArray[n].jj)
                            {
                                //заполняю нулём строку только те ячейки где значение меньше нуля
                                if (OporniyPlan[IndexMinElArray[n].ii, i2] < 0 /*|| OporniyPlan[IndexMinElArray[n].ii, i2] == -1*/)
                                { OporniyPlan[IndexMinElArray[n].ii, i2] = 0; }

                            }
                        }
                    }
                }

                n = n + 1;

            }

            //вывод опорного плана
            for (int qq = 0; qq < SuplierArray.Length; ++qq)
            {
                for (int ww = 0; ww < CustomerArray.Length; ww++)
                {
                    Console.Write("{0}\t", OporniyPlan[qq, ww]);
                }
                Console.WriteLine();
            }

            return OporniyPlan;
        }

        //Расчёт L(x) нужен опорный план, тариф и длина ai bi
        public static void TargeFunction(int[,] OpPlan, int[,] Trafic, int a, int b)
        {
            int Lx = 0;
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    if (OpPlan[i, j] != 0)
                    {
                        Lx = Lx + OpPlan[i, j] * Trafic[i, j];
                    }
                }
            }
            Console.WriteLine($"L(x) = {Lx}"); ;

        }
    }

    public class GetInfo
    {


        public int[] Supplier()
        {
            Console.Write("\nВведите количество складов: ");

            //Количество элементов склада, то есть сколько (ai)
            int a = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nВвод данных склада");
            //Массив хронящий данные склада (ai)
            int[] arraySupplier = new int[a];

            //Цикл заполнения массива
            for (int i = 0; i < a; i++)
            {
                Console.Write($"Склад {i + 1}:  ");
                arraySupplier[i] = Convert.ToInt32(Console.ReadLine());
            }

            return arraySupplier;
        }

        public int[] Customers()
        {
            Console.Write("\nВведите количество потребителей:");

            //Количество элементов склада, то есть сколько (ai)
            int a = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nВвод данных потребителей");
            //Массив хронящий данные поребителей (ai)
            int[] arrayCustomer = new int[a];

            //Цикл заполнения массива
            for (int i = 0; i < a; i++)
            {
                Console.Write($"Потребитель {i + 1}:   ");
                arrayCustomer[i] = Convert.ToInt32(Console.ReadLine());
            }

            return arrayCustomer;
        }

        public int[,] DataPlans(int[] SupplierArray, int[] CustomersArray)
        {

            int a = SupplierArray.Length;
            int b = CustomersArray.Length;

            Console.WriteLine("\nВвод тарифного плана\n");
            int[,] TarifPlan = new int[a, b];

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    Console.Write($"Введите тарифный для ячейки {i + 1} {j + 1}: ");
                    TarifPlan[i, j] = Convert.ToInt32(Console.ReadLine());
                }

            }

            return TarifPlan;
        }


    }
}

