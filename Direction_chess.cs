using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Program
{
    class Progarm
    {
        static void Main(string[] args)
        {
            Console.WriteLine("作者:大米菌\n欢迎游玩\n----------------");
            int i, j, height, width,
                stx, sty, trix, triy, trit, tril, tl, cl, cx, cy, choice;//加强横坐标,加强纵坐标,触发横坐标,触发纵坐标,触发类型,触发等级,触发者等级,改变横坐标,改变y坐标,选择
            Console.WriteLine("输入棋盘高度");
            height = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("输入棋盘宽度");
            width = Convert.ToInt16(Console.ReadLine());
            int[,,] runing = new int[height,width,2]; //列,行,属性
            for (i = 0; i < height; i++)  //为数组添加元素
            {
                for (j = 0; j < width; j++)
                {
                    byte[] buffer = Guid.NewGuid().ToByteArray();//随机数
                    int iSeed = BitConverter.ToInt32(buffer, 0);
                    Random random = new Random(iSeed);
                    runing[i, j, 0] = random.Next(1, 6);
                    runing[i, j, 1] = 1;
                }
            }
            int g(int a, int b, int c) //获取running数组元素
            {
                return runing[a, b, c];
            }
            void c(int a,int b,int c,int d)  //改变running数组元素
            {
                runing[a, b, c] = d;
            }
            string[] type = new string[] { "□", "＞", "∧", "＜", "∨", "⊙" };//渲染类型数组;0空格子,5圆格子
            string[] level = new string[] { "¹", "²", "³" }; //渲染等级数组;1一级,2二级,3三级
            void rendering()  //渲染
            {
                for (i = 0; i < height; i++)
                {
                    string a = "";
                    for (j = 0; j < width; j++)
                    {
                        a += type[g(i, j, 0)] + level[g(i, j, 1) - 1];
                    }
                    Console.WriteLine(a);
                }
            }
            List<List<int>> storage = new List<List<int>>();//存储列表
            int score = 0;
            void add()
            {
                List<int> v = new List<int>() { cx, cy, g(cy,cx,0), g(cy,cx,1), cl };//触发横坐标,触发纵坐标,触发类型,触发等级,触发者等级
                storage.Add(v);
            }
            void cplace()
            {
                c(cy, cx, 0, trit);
                c(cy, cx, 1, cl);
            }
            void unplace()
            {
                c(triy, trix, 0, trit);
                c(triy, trix, 1, cl);
            }
            int[,] type_list = new int[4, 4] { { 1, 0, width, 1 }, { 0, -1, 1, -1 }, { -1, 0, 1, -1 }, { 0, 1, height, 1 } };
            int gt(int a, int b)
            {
                return type_list[a, b];
            }
            void st()
            {
                Console.WriteLine("输入加强横坐标");
                stx = Convert.ToInt16(Console.ReadLine()) - 1;
                Console.WriteLine("输入加强纵坐标");
                sty = Convert.ToInt16(Console.ReadLine()) - 1;
                if ((stx < width && stx >= 0 && sty < height && sty >= 0) == false)
                {
                    Console.WriteLine("请输入正确的数字");
                    st();
                }
                else if (g(sty, stx, 0) == 0)
                {
                    Console.WriteLine("不能加强空位置");
                    st();
                }
                else if (g(sty, stx, 1) == 3)
                {
                    Console.WriteLine("超出加强范围");
                    st();
                }
            }
            void tri()
            {
                Console.WriteLine("输入触发横坐标");
                trix = Convert.ToInt16(Console.ReadLine()) - 1;
                Console.WriteLine("输入触发纵坐标");
                triy = Convert.ToInt16(Console.ReadLine()) - 1;
                if ((trix < width && trix >= 0 && triy < height && triy >= 0) == false)
                {
                    Console.WriteLine("请输入正确的数字");
                    tri();
                }
                else if(g(triy, trix, 0) == 0)
                {
                    Console.WriteLine("不能触发空位置");
                    tri();
                }
            }
            while (true)//游戏主循环
            {
                rendering();
                Console.WriteLine("得分:{0}", score);
                Random random = new Random();
                if (random.Next(0, 3) < 2)//3分之2的概率
                {
                    st();//加强
                    runing[sty, stx, 1] += 1;
                    rendering();
                }
                tri();//触发
                trit = g(triy, trix, 0);
                tril = g(triy, trix, 1);
                tl = 1;
                List<int> v = new List<int>() { trix, triy, trit, tril, tl };//触发横坐标,触发纵坐标,触发类型,触发等级,触发者等级
                storage.Add(v);
                while (storage.Count != 0)
                {
                    trix = storage[0][0];
                    triy = storage[0][1];
                    trit = storage[0][2];
                    tril = storage[0][3];
                    tl = storage[0][4];
                    cl = tril - 2 + tl;
                    c(triy, trix, 0, 0);//删除格子
                    c(triy, trix, 1, 1);//清除等级
                    score += cl + 1;//加分
                    if (cl > 0)
                    {
                        if (trit != 5)
                        {
                            int choice_trit = trit - 1;
                            cx = trix + gt(choice_trit, 0);
                            cy = triy + gt(choice_trit, 1);
                            int[] xy = new int[2] { cx, cy };
                            choice = choice_trit % 2;
                            if (gt(choice_trit, 3) * xy[choice] < gt(choice_trit, 2))
                            {
                                if (g(cy,cx,0)==trit | g(cy, cx, 0) == 5)
                                {
                                    add();
                                }
                                else if (g(cy, cx, 0) == 0)
                                {
                                    cplace();
                                }
                                else
                                {
                                    unplace();
                                }
                                
                            }
                            else
                            {
                                unplace();
                            }
                        }
                        else
                        {
                            int[,] circle = new int[4, 3] { { trix, 1, width, }, { triy, -1, 1, }, { trix, -1, 1, }, { triy, 1, height, } };
                            for (i = 0; i < 4; i++)
                            {
                                cx = trix + gt(i, 0);
                                cy = triy + gt(i, 1);
                                choice = circle[i, 0] + circle[i, 1];
                                if (choice * circle[i, 1] < circle[i, 2])
                                {
                                    if (g(cy, cx, 0) == 0)
                                    {
                                        cplace();
                                    }
                                    else
                                    {
                                        add();
                                    }
                                }
                                else
                                {
                                    if (g(triy, trix, 0) == 5)
                                    {
                                        int vvv = Convert.ToInt16(g(triy, trix, 1) + cl == 4);
                                        List<int> vv = new List<int>() { trix, triy, 5, g(triy, trix, 1), cl-vvv };//触发横坐标,触发纵坐标,触发类型,触发等级,触发者等级
                                        storage.Add(vv);
                                    }
                                    else
                                    {
                                        unplace();
                                    }
                                }
                            }
                        }
                    }
                    storage.RemoveAt(0);
                }
            }
         }
    }
}