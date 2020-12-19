using System;
using System.Collections.Generic;


namespace Program
{
    class Progarm
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("作者:大米菌\n欢迎游玩\n----------------");
            int height,//棋盘高度
                width,//棋盘宽度
                ux,//升级横坐标
                uy,//升级纵坐标
                tx,//触发横坐标
                ty,//触发纵坐标
                tt,//触发类型
                ptl,//被触发等级
                atl,//主触发等级
                cl,//改变等级
                cx,//改变横坐标
                cy,//改变纵坐标
                choice,//选择
                epl = 1,//epl每次升级等级
                etl = 1,//每次触发等级
                score = 0,//分数
                player = 0,
                maxplayer;
            Console.WriteLine("输入玩家数量");
            maxplayer = Convert.ToInt16(Console.ReadLine());//以后加数值检测
            Console.WriteLine("输入棋盘高度");
            height = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("输入棋盘宽度");
            width = Convert.ToInt16(Console.ReadLine());
            int[,,] runing = new int[height, width, 2]; //列,行,属性0
            int get(int a, int b, int c)//获取running数组元素
            {
                return runing[a, b, c];
            }
            void change(int a, int b, int c, int d)//改变running数组元素
            {
                runing[a, b, c] = d;
            }
            int random()
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                return BitConverter.ToInt32(buffer, 0);
            }
            for (int i = 0; i < height; i++)  //为数组添加元素
            {
                for (int j = 0; j < width; j++)
                {
                    Random ran = new Random(random());
                    change(i, j, 0, ran.Next(1, 6));
                    change(i, j, 1, 1);
                }
            }
            char[] type = new char[] { '□', '＞', '∧', '＜', '∨', '⊙' };//渲染类型数组
            char[] level = new char[] { '1', '2', '3', '4', '5', '6', '7', '8' }; //渲染等级数组
            ConsoleColor[] colors = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.Red, ConsoleColor.DarkYellow, ConsoleColor.Blue, ConsoleColor.Green };
            int[,] color_list = new int[height,width];
            void rendering()  //渲染
            {
                //Console.Clear();//玩时使用
                Console.Write('\n');//debug时使用
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Console.BackgroundColor = colors[color_list[i,j]];
                        Console.Write(type[get(i, j, 0)]);
                        Console.Write(level[get(i, j, 1) - 1]);
                    }
                    Console.Write('\n');
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            List<List<int>> storage = new List<List<int>>();//存储列表
            void add()
            {
                List<int> v = new List<int>() { cx, cy, get(cy, cx, 0), get(cy, cx, 1), cl };//触发横坐标,触发纵坐标,触发类型,被触发等级,主触发等级
                storage.Add(v);
            }
            void cplace()
            {
                change(cy, cx, 0, tt);
                change(cy, cx, 1, cl);
            }
            void unplace()
            {
                change(ty, tx, 0, tt);
                change(ty, tx, 1, cl);
            }
            int[,] type_list = new int[4, 4] { { 1, 0, width, 1 }, { 0, -1, 1, -1 }, { -1, 0, 1, -1 }, { 0, 1, height, 1 } };
            int gt(int a, int b)
            {
                return type_list[a, b];
            }
            void st()
            {
                Console.WriteLine("输入加强横坐标");
                ux = Convert.ToInt16(Console.ReadLine()) - 1;
                Console.WriteLine("输入加强纵坐标");
                uy = Convert.ToInt16(Console.ReadLine()) - 1;
                if (!(ux < width && ux >= 0 && uy < height && uy >= 0))
                {
                    Console.WriteLine("请输入正确的数字");
                    st();
                }
                else if (color_list[uy, ux] != player)
                {
                    Console.WriteLine("不是你的区域");
                    st();
                }
                else if (get(uy, ux, 0) == 0)
                {
                    Console.WriteLine("不能加强空位置");
                    st();
                }
                else if ((get(uy, ux, 0) != 5 && get(uy, ux, 1) + epl > 3) | (get(uy, ux, 1) + epl > 8))//圆的最大等级是8
                {
                    Console.WriteLine("超出加强范围");
                    st();
                }
            }
            void tri()
            {
                Console.WriteLine("输入触发横坐标");
                tx = Convert.ToInt16(Console.ReadLine()) - 1;
                Console.WriteLine("输入触发纵坐标");
                ty = Convert.ToInt16(Console.ReadLine()) - 1;
                if (!(tx < width && tx >= 0 && ty < height && ty >= 0))
                {
                    Console.WriteLine("请输入正确的数字");
                    tri();
                }
                else if (color_list[ty, tx] != player)
                {
                    Console.WriteLine("不是你的区域");
                    tri();
                }
                else if (get(ty, tx, 0) == 0)
                {
                    Console.WriteLine("不能触发空位置");
                    tri();
                }
            }
            bool pro(int a, int b)//  a/b 的概率
            {
                Random ran = new Random(random());
                return ran.Next(b) < a;
            }
            int fan(int a)
            {
                if (a == 1 | a == 2)
                {
                    return a + 2;
                }
                else
                {
                    return a - 2;
                }
            }
            //游戏开始//
            for(player = 1; player <= maxplayer; player++)
            {
                rendering();
                Console.WriteLine("选择玩家{0}的初始位置",player);
                Console.WriteLine("输入x坐标");
                cx = Convert.ToInt16(Console.ReadLine())-1;
                Console.WriteLine("输入y坐标");
                cy = Convert.ToInt16(Console.ReadLine())-1;
                color_list[cy, cx] = player;
            }
            while (true)//游戏主循环
            {
                player += 1;
                if (player > 2)
                {
                    player = 1;
                }
                rendering();
                Console.WriteLine("玩家:{0}", player);
                Console.WriteLine("得分:{0}", score);
                if (pro(3, 4))//  3/4的概率
                {
                    st();//加强
                    runing[uy, ux, 1] += epl;
                    color_list[uy, ux] = player;
                    rendering();
                }
                Console.WriteLine("是否进行触发（触发请按enter，不触发请按其他键）");
                if (Console.ReadKey(false).Key==ConsoleKey.Enter)
                {
                    tri();//触发
                    tt = get(ty, tx, 0);
                    ptl = get(ty, tx, 1);
                    List<int> v = new List<int>() { tx, ty, tt, ptl, etl/*每次触发等级*/ };//触发横坐标,触发纵坐标,触发类型,触发等级,触发者等级
                    storage.Add(v);
                }
                while (storage.Count != 0)
                {
                    rendering();
                    tx = storage[0][0];
                    ty = storage[0][1];
                    tt = storage[0][2];
                    ptl = storage[0][3];
                    atl = storage[0][4];
                    cl = ptl - 2 + atl;
                    change(ty, tx, 0, 0);//删除格子
                    change(ty, tx, 1, 1);//清除等级
                    color_list[ty, tx] = player;
                    score += cl + 1;//加分
                    if (cl > 0)
                    {
                        if (tt != 5)
                        {
                            int choice_trit = tt - 1;
                            cx = tx + gt(choice_trit, 0);
                            cy = ty + gt(choice_trit, 1);
                            int[] xy = new int[2] { cx, cy };
                            choice = choice_trit % 2;
                            if (gt(choice_trit, 3) * xy[choice] < gt(choice_trit, 2))
                            {
                                if (get(cy, cx, 0) == tt | get(cy, cx, 0) == 5)
                                {
                                    add();
                                }
                                else if (get(cy, cx, 0) == 0)
                                {
                                    cplace();
                                }
                                else
                                {
                                    change(ty, tx, 0, fan(tt));
                                    change(ty, tx, 1, cl);//遇到非同类和圆改变成相反方向**
                                }

                            }
                            else
                            {
                                change(ty, tx, 0, fan(tt));
                                change(ty, tx, 1, cl);//边界改变成相反方向
                            }
                        }
                        else
                        {
                            int[,] circle = new int[4, 3] { { tx, 1, width, }, { ty, -1, 1, }, { tx, -1, 1, }, { ty, 1, height, } };
                            int all = cl + 1, f, e;//总数,容数:4,余数
                            f = all / 4;
                            e = all - 4 * f;
                            for (int i = 0; i < 4; i++)
                            {
                                if (pro(e, 4 - i))
                                {
                                    cl = f + 1;
                                    e -= 1;
                                }
                                else
                                {
                                    cl = f;
                                }
                                bool z = cl != 0;
                                cx = tx + gt(i, 0);
                                cy = ty + gt(i, 1);
                                choice = circle[i, 0] + circle[i, 1];
                                if (choice * circle[i, 1] < circle[i, 2])
                                {
                                    if (get(cy, cx, 0) == 0)
                                    {
                                        if (z)
                                        {
                                            cplace();
                                        }
                                    }
                                    else
                                    {
                                        add();
                                    }
                                }
                                else
                                {
                                    if (get(ty, tx, 0) == 5)
                                    {
                                        List<int> vv = new List<int>() { tx, ty, 5, get(ty, tx, 1), cl };//触发横坐标,触发纵坐标,触发类型,触发等级,触发者等级
                                        storage.Add(vv);
                                    }
                                    else if (z)
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