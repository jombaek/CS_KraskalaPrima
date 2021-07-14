using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_KraskalaPrima
{
    class Program
    {
        public struct Vertex
        {
            public int name;
            public int met;
            public Vertex(int name, int met)
            {
                this.name = name;
                this.met = met;
            }
        }
        public struct Edge
        {
            public int v1;
            public int v2;
            public int weight;
            public Edge(int v1, int v2, int weight)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.weight = weight;
            }
        }
        static List<Edge> Edge_Copy(List<Edge> edges1)
        {
            List<Edge> edges = new List<Edge>();
            foreach (var item in edges1)
            {
                edges.Add(item);
            }
            return edges;
        }


        /*Алгоритм Прима*/
        static void Prim(List<Edge> edgest1)
        {
            List<Edge> edgest = Edge_Copy(edgest1); //создание копии массива ребер, содержащихся в массиве
            List<int> used = new List<int>(); //использование вершины
            List<int> unused = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 }; //неиспользованные вершины
            Edge Min_Edge = new Edge(0, 0, 0); //пустой экземпляр структуры "ребро"
            bool flag = false;
            List<Edge> MST = new List<Edge>(); //остов минимального веса
            int MST_weight = 0, current = 0; //вес остова и текущая вершина
            while (unused.Count > 0) //пока не просмотрены все вершины
            {
                flag = true;
                Min_Edge.weight = int.MaxValue; //присвоить минимальному ребру максимально возможный вес
                foreach (var item in edgest) //посмотреть все ребра в списке
                {
                    if (item.weight < Min_Edge.weight)
                    {
                        Min_Edge = item;
                    }
                }
                edgest.Remove(Min_Edge); //получить минимальное и удалить его из списка ребер как посмотренное
                if (unused.Contains(Min_Edge.v1) && used.Contains(Min_Edge.v2) || //если ребро начинается с непросмотренной вершины
                    used.Contains(Min_Edge.v1) && unused.Contains(Min_Edge.v2) || //заканчивается непросмотренной вершины
                    unused.Contains(Min_Edge.v1) && unused.Contains(Min_Edge.v2)) //или содержит два непросмотренные вершины
                {
                    if (unused.Contains(Min_Edge.v1) && used.Contains(Min_Edge.v2)) current = Min_Edge.v1; //получить непросмотренную вершину НАЧАЛЬНУЮ
                    else if (used.Contains(Min_Edge.v1) && unused.Contains(Min_Edge.v2)) current = Min_Edge.v2; //получить непросмотренную вершину КОНЕЦ
                    else
                    {
                        unused.Remove(Min_Edge.v1); //удалить начало ребра из непросмотренных
                        used.Add(Min_Edge.v1); //внести в просмотренные
                        current = Min_Edge.v2; //назначить текущий конец ребра
                    }
                    flag = false; //сменить флаг
                }
                if (flag) continue; //если обе вершины уже посмотрены, перейти к следующей итерации
                unused.Remove(current); //удалить вершины из непросмотренных
                used.Add(current); //добавить ее в просмотренные
                MST.Add(Min_Edge); //добавить ребро в остав минимального веса
                MST_weight += Min_Edge.weight; //увеличить вес остава
            }
            Console.WriteLine("Вес минимального оставного дерева = {0}\n", MST_weight);
            Console.WriteLine("Остов минимального веса имеет вид: ");
            See(MST);
        }

        /*Алгоритм Краскала*/
        static void Kraskala(List<Edge> edges1)
        {
            List<Edge> edges = Edge_Copy(edges1); //создание копии массива ребер, содержащихся в графе
            edges.Sort((a, b) =>
            a.weight.CompareTo(b.weight));  //сортировка списка ребер с помощью метода сортировки списка
            Edge min; //текущее минимальное ребро
            bool flag1 = false, flag2 = false; //флаги для фиксации получения меток
            List<Vertex> vertices = new List<Vertex>(); //лист, содержащий вершины графа
            for (int i = 0; i < 8; i++)
            {
                vertices.Add(new Vertex(i, -1)); //на начальном этапе все метки равны -1
            }
            List<Edge> MST = new List<Edge>(); //остов минимального веса
            int MST_weight = 0;
            int color = 1; //метка "цвета" вершины
            while (MST.Count < 7) //пока число ребер не станет (число вершин - 1)
            {
                min = edges[0]; //получаем текущее минимальное ребро
                edges.Remove(min); //удаляем его из списка ребер
                flag1 = flag2 = false;
                int vert1 = -2; //метка первой вершины
                int vert2 = -2; //метка второй вершины
                foreach (var item in vertices) //проходимся по всем вершинам
                {
                    if (item.name == min.v1) vert1 = item.met; //ищем вершину, с которой начинается минималное ребро, получаем ее метку
                    if (item.name == min.v2) vert2 = item.met; //ищем вершину, в которой заканчивается минималное ребро, получаем ее метку
                    if (vert1 != 2 && vert2 != -2) break; //если обе вершины найдены, выйти
                }
                if (vert1 == -1 && vert2 == -1) //если вершины не принадлежат никакой из существующих компонетов
                {
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (vertices[i].name == min.v1) //ищем вершину, с которой начинается минималное ребро
                        {
                            vertices.Remove(vertices[i]); //удаляем ее из списка
                            vertices.Add(new Vertex(min.v1, color)); //добавляем с новой меткой
                            flag1 = true;
                        }
                        if (vertices[i].name == min.v2) //ищем вершину, в которой заканчивается минималное ребро
                        {
                            vertices.Remove(vertices[i]); //удаляем ее из списка
                            vertices.Add(new Vertex(min.v2, color)); //добавляем с новой меткой
                            flag2 = true;
                        }
                        if (flag2 && flag1) break; //как толко метки изменены - выйти
                    }
                }
                else if (vert1 == vert2) continue; //если вершина принадлежит одному компоненты связности, продожить поиск
                else
                {
                    if (vert1 == -1 || vert2 == -1) //если одна из вершин принадлежит не принадлежит никакой из существующих компонент
                    {
                        int not_Minus = 0; //получить метку, которая равна -1
                        if (vert1 == -1) not_Minus = vert2;
                        else not_Minus = vert1;
                        int name = -4; //получить имя вершины, не принадлежащий никакой из существующих компонент
                        if (not_Minus == vert1) name = min.v2;
                        else name = min.v1;
                        foreach (var item in vertices) //найти эту вершину в списке вершин
                        {
                            if (item.name == name)
                            {
                                vertices.Remove(item); //удалить ее из списка
                                vertices.Add(new Vertex(name, not_Minus)); //добавить с новой меткой
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < vertices.Count; i++) //если вершины из разных компонентов связности
                        {
                            if (vertices[i].met == vert2) //найти все вершины, принадлежащие второму компоненту
                            {
                                int name = vertices[i].name;
                                vertices.Remove(vertices[i]);
                                vertices.Add(new Vertex(name, vert1)); //присвоить им метки первого компонента
                            }
                        }
                    }
                }
                if (vert1 == --color || vert2 == --color) color += 1; //если цвет использован, задать другой
                MST.Add(min); //добавить ребро в остов минимального веса
                MST_weight += min.weight; //увеличить вес остова
            }
            Console.WriteLine("Вес минимального оставного дерева = {0}\n", MST_weight);
            Console.WriteLine("Остов минимального веса имеет вид: ");
            See(MST); //вывести ее ребра и их вес
        }

        static List<Edge> Edge_Full(int[,] vs, int count) //заполнение массива ребер по заданной матрице смежности
        {
            List<Edge> edges = new List<Edge>(); //список ребер, который нужно наполнить
            for (int i = 0; i < count; i++)
            {
                for (int j = i; j < count; j++)
                {
                    if (vs[i, j] != 0)
                    {
                        edges.Add(new Edge(i, j, vs[i, j])); //добавление нового экземпляра структуры "ребро"
                    }
                }
            }
            See(edges); //вывод списка ребер
            return edges;
        }
        static void See(int[,] vs) //вывод элементов матрицы
        {
            for (int i = 0; i < Math.Sqrt(vs.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(vs.Length); j++)
                {
                    Console.Write("{0,4}", vs[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void See(List<Edge> edges) //вывод элементов списка
        {
            foreach (var item in edges)
            {
                Console.WriteLine("{0} - {1} : {2} ", item.v1 + 1, item.v2 + 1, item.weight); //вывод вершины, принадлежащих ребрам и веса соответствуещего ребра
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Матрица графа:\n\n");
            int[,] vs =
            {
                {0,2,0,4,0,0,3,0},
                {2,0,1,1,2,0,0,8},
                {0,1,0,0,7,0,0,0},
                {4,1,0,0,0,5,1,0},
                {0,2,7,0,0,0,2,2},
                {0,0,0,5,0,0,1,0},
                {3,0,0,1,2,1,0,2},
                {0,8,0,0,2,0,2,0}
            };
            See(vs);
            Console.WriteLine("Дуги графа выглядят так:\n");
            List<Edge> edges = Edge_Full(vs, 8);
            do
            {
                Console.WriteLine("\nВыберите алгоритм построения остова:");
                Console.WriteLine("1. Алгоритм Прима");
                Console.WriteLine("2. Алгоритм Крускала");
                Console.Write("Ваш выбор: ");
                int choose = int.Parse(Console.ReadLine());
                switch (choose)
                {
                    case 1:
                        Prim(edges);
                        break;
                    case 2:
                        Kraskala(edges);
                        break;
                    default:
                        Console.WriteLine("Ошибка!");
                        break;
                }
                Console.WriteLine("Для выхода введите Esc.");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
