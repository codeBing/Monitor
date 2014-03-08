using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanXing
{
    class ComputeHanXing
    {
        //平滑函数
        private static double[,] smooth(int[,] ori_value, int smooth_length)
        {
            //初始化my_smooth
            double[,] my_smooth = new double[ori_value.GetLength(0), ori_value.GetLength(1)];
            //包含自身的平滑
            for (int index = 0; index < ori_value.GetLength(0) * ori_value.GetLength(1); index++)
            {
                int pos = index - smooth_length + 1; 
                double sum = 0;
                //不足平滑时间长度的数据按实际时间长度平滑。
                if (index < smooth_length )
                {
                    pos = 0;
                }
                for (int j = pos; j <= index; j++)
                {
                    sum += ori_value[j / 24, j % 24];
                }
                my_smooth[index / 24, index % 24] = sum / (index - pos + 1);
            }
            return my_smooth;
        }
        //同比方法，标准差
        private static double[,] cross_sd(double[,] smooth_value, int compare_length)
        {
            //初始化
            double[,] my_cross_sd = new double[smooth_value.GetLength(0),smooth_value.GetLength(1)];
            //double ;
            int no_count = smooth_value.GetLength(0) < 3 ? smooth_value.GetLength(0) : 3;
            //前三天的不计,都记成0.
            for (int index = 0; index < no_count; index++)
            {
                for (int j = 0; j < 24; j++)
                    { my_cross_sd[index, j] = 0; }
            }
            //计算同比，不包含当天。就是把当天的和之前的平均值的差来和标准差比较。
            for (int index = 3; index < smooth_value.GetLength(0); index++ )
            {
                //内循环是每个小时的，外循环是每一天的
                for(int j=0; j < smooth_value.GetLength(1); j++)
                {
                    int pos = index - compare_length;
                    double sum = 0;
                    //不足比较时长的按实际时长进行比较
                    if(index < compare_length)
                        {pos = 0;}
                    //不包含本身，进行具体的同比,也就是不同列的同一行对比
                    for (int k = pos; k < index; k++)
                        {sum += smooth_value[k,j];}
                    //得到平均值
                    double mean = sum / (index-pos);
                    //计算标准差
                    sum = 0;
                    for (int k = pos; k < index; k++)
                        {sum += (smooth_value[k, j] - mean) * (smooth_value[k, j] - mean);}
                    double sd = Math.Pow(sum / (index - pos),0.5);
                    //如果之前的标准差是零，为了可运算，将其对应的数变成0；
                    if (sd != 0)
                        { my_cross_sd[index, j] = (smooth_value[index, j] - mean) / sd; }
                    else
                        { my_cross_sd[index, j] = 0; }
                }
            }
            return my_cross_sd;
        }

        //同比方法，平均差
        private static double[,] cross_mean(double[,] smooth_value, int compare_length)
        {
            //初始化
            double[,] my_cross_mean = new double[smooth_value.GetLength(0), smooth_value.GetLength(1)];
            //double ;
            int no_count = smooth_value.GetLength(0) < 3 ? smooth_value.GetLength(0) : 3;
            //前三天的不计,都记成0.
            for (int index = 0; index < no_count; index++)
            {
                for (int j = 0; j < 24; j++)
                { my_cross_mean[index, j] = 0; }
            }
            //计算同比，不包含当天。就是把当天的和之前的平均值的差来和标准差比较。
            for (int index = 3; index < smooth_value.GetLength(0); index++)
            {
                //内循环是每个小时的，外循环是每一天的
                for (int j = 0; j < smooth_value.GetLength(1); j++)
                {
                    int pos = index - compare_length;
                    double sum = 0;
                    //不足比较时长的按实际时长进行比较
                    if (index < compare_length)
                    { pos = 0; }
                    //不包含本身，进行具体的同比,也就是不同列的同一行对比
                    for (int k = pos; k < index; k++)
                    { sum += smooth_value[k, j]; }
                    //得到平均值
                    double mean = sum / (index - pos);
                    //计算标准差
                    sum = 0;
                    for (int k = pos; k < index; k++)
                    { sum += (smooth_value[k, j] - mean) * (smooth_value[k, j] - mean); }
                    double sd = Math.Pow(sum / (index - pos), 0.5);
                    //如果之前的标准差是零，为了可运算，将其对应的数变成0；
                    if (sd != 0)
                    { my_cross_mean[index, j] = (smooth_value[index, j] - mean) / sd; }
                    else
                    { my_cross_mean[index, j] = 0; }
                }
            }
            return my_cross_mean;
        }

        public static string result(string ori_string)
        {
            string[] ori_split;
            //定位到“，”号,使用split函数分解
            ori_split = ori_string.Split(',');
            //把分解的字符串变成数值数组。行是日期，列是小时
            int[,] ori_value = new int[ori_split.Length / 24, 24];
            for (int index = 0; index < ori_split.Length; index++)
            {
                //调试
                //Console.WriteLine(item);
                //i是行、是日期，j是列、是小时。存进二维数组中
                int i = index / 24;
                int j = index % 24;
                ori_value[i, j] = int.Parse(ori_split[index]);
            }
            //平滑
            double[,] smooth_data = new double[ori_split.Length / 24, 24];
            smooth_data = smooth(ori_value, 8);
            //同比,标准差
            double[,] sd_data = new double[ori_split.Length / 24, 24];
            sd_data = cross_sd(smooth_data, 7);

            //返回逗号相隔的数据形成的字符串

            string result_string = "";
            foreach (double item in sd_data)
            {
                //保留2位小数点
                double temp = Math.Floor(item*100)/100.0;
                result_string += "," + temp.ToString();
            }
            //去掉最开头多余的“，”号
            return result_string.Remove(0, 1);
        }

    }
}
