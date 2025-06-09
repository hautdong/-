using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace staPos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 设置预设值
            textBox1.Text = "5153.71277046";
            textBox2.Text = "4.1115998360700002e-009";
            textBox3.Text = "0.00286006834358";
            textBox4.Text = "1.70530256582e-012";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "4800";
            textBox8.Text = "0";
            textBox9.Text = "1.22639730096";
            textBox10.Text = "0.0053100715158500003";
            textBox11.Text = "-1.6819446292500000";
            textBox12.Text = "-5.5264681577700003e-006";
            textBox13.Text = "1.1192634701700000e-005";
            textBox14.Text = "-105.43750000000000";
            textBox15.Text = "175.34375000000000";
            textBox16.Text = "-9.6857547759999998e-008";
            textBox17.Text = "-7.8231096267699997e-008";
            textBox18.Text = "0.97432927738800001";
            textBox39.Text = "1.8643633724999999e-010";
            textBox40.Text = "-2.9080127721900002";
            textBox41.Text = "-7.7124641122299999e-009";
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        //牛顿迭代
        private double SolveKeplerNewton(double meanAnomaly, double eccentricity, double tolerance = 1e-12, int maxIterations = 100)
        {
            // Ek 的初始猜测值（可以是 meanAnomaly 本身或其他近似值）
            double Ek = meanAnomaly;

            for (int i = 0; i < maxIterations; i++)
            {
                // 函数 f(Ek) = Ek - e * sin(Ek) - Mk，我们要找到 f(Ek) = 0 的根
                double f = Ek - eccentricity * Math.Sin(Ek) - meanAnomaly;
                // f(Ek) 的导数 f'(Ek) = 1 - e * cos(Ek)
                double fPrime = 1 - eccentricity * Math.Cos(Ek);

                // 避免除以零的情况
                if (Math.Abs(fPrime) < double.Epsilon)
                {
                    // 处理 fPrime 接近零的情况，例如跳出循环或返回错误/默认值
                    break;
                }

                // 牛顿迭代法的步长
                double deltaEk = f / fPrime;
                // 更新 Ek
                Ek -= deltaEk;

                // 如果步长小于容差，则认为已收敛
                if (Math.Abs(deltaEk) < tolerance)
                {
                    return Ek; // 在容差范围内找到解
                }
            }
            // 如果达到最大迭代次数仍未收敛，返回当前的最佳猜测值。
            // 对于导航应用，通常最好返回最后计算的值并向上游处理潜在的错误。
            return Ek;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取输入值
                double roota = double.Parse(textBox1.Text);  //半轴长
                double deltaN = double.Parse(textBox2.Text); //摄动改正数
                double a0 = double.Parse(textBox3.Text);    //时钟修正0
                double a1 = double.Parse(textBox4.Text);    //时钟修正1
                double a2 = double.Parse(textBox5.Text);    //时钟修正2
                double toc = double.Parse(textBox6.Text);    // 卫星钟基准时间
                double time= double.Parse(textBox7.Text);    // 观测时间
                double toe = double.Parse(textBox8.Text);    // 星历基准时间
                double M0 = double.Parse(textBox9.Text);      //参考时刻的平近点角
                double e1 = double.Parse(textBox10.Text);   //轨道离心率
                double omega = double.Parse(textBox11.Text);    //近地点角距
                double Cuc = double.Parse(textBox12.Text); //轨道延迹方向上周期改正余弦振幅
                double Cus = double.Parse(textBox13.Text); //轨道延迹方向上周期改正正弦振幅
                double Crs = double.Parse(textBox14.Text); //对轨道半径正弦的修正值
                double Crc = double.Parse(textBox15.Text); //在轨道径向方向上周期改正余余弦的振幅
                double Cic = double.Parse(textBox16.Text); //轨道倾角周期改正余弦项振幅
                double Cis = double.Parse(textBox17.Text); //轨道倾角周期改正正弦项振幅
                double Io = double.Parse(textBox18.Text); //参考时间轨道倾角
                double Idot = double.Parse(textBox39.Text); //轨道倾角变化率
                double omega0 = double.Parse(textBox40.Text);//参考时刻升交点赤径主项
                double omgadot = double.Parse(textBox41.Text); //升交点赤径在赤道平面中的长期变化

                // 计算平均角速度 n0
                double GM = 3.986005e14;  // 地球引力常数 (m^3/s^2)
                double roota1 = roota * roota;  // roota * 10^3
                double n0 = Math.Sqrt(GM/ Math.Pow(roota1, 3))  ;  // 修正后的公式               
                // 计算最终结果 n = n0 + Δn 
                double n = n0 + deltaN;
                // 显示结果
                textBox19.Text = n0.ToString(); 

                //计算归化时间tA(卫星钟差Δt):
                double deltaT =a0 + a1 * (time - toc) + a2 * Math.Pow((time - toc), 2);
                textBox20.Text = deltaT.ToString(); 

                //计算归化时间t
                double t = time - deltaT;
                textBox21.Text = t.ToString();

                //计算归化时间tk
                double tk = time - toe;
                textBox22.Text = tk.ToString();

                //计算观测时刻卫星平近点Mk
                double Mk = M0 + n * tk;
                textBox23.Text = Mk.ToString();

                //计算偏近点角Ek
             
                double Ek = SolveKeplerNewton(Mk, e1);
                textBox24.Text = Ek.ToString(); // 以10位小数显示 Ek

                //计算偏近点角Vk
                double numerator_Vk = Math.Sqrt(1 - e1 * e1) * Math.Sin(Ek);
                double denominator_Vk = Math.Cos(Ek) - e1;
                double Vk = Math.Atan2(numerator_Vk, denominator_Vk);
                textBox25.Text = Vk.ToString();

                //计算升交距角Ak
                double Ak = Vk + omega;
                textBox26.Text = Ak.ToString();

                //计算摄动改正项u
                double twoAk = 2 * Ak; 
                double u = Cuc * Math.Cos(twoAk) + Cus * Math.Sin(twoAk);
                textBox27.Text = u.ToString();

                //计算摄动改正项r
                double r = Crc * Math.Cos(twoAk) + Crs * Math.Sin(twoAk);
                textBox28.Text = r.ToString();

                //计算摄动改正项i
                double i = Cic * Math.Cos(twoAk) + Cis * Math.Sin(twoAk);
                textBox29.Text = i.ToString();

                //改正后升交距角Uk
                double Uk = Ak + u;
                textBox30.Text = Uk.ToString();

                //改正后卫星矢量Rk
                double Rk = roota1 * (1 - e1 * Math.Cos(Ek)) + r;
                textBox31.Text = Rk.ToString();

                //改正后轨道倾角Ik
                double Ik = Io + i + Idot * tk;
                textBox32.Text = Ik.ToString();

                //计算卫星在轨道平面坐标Xk
                double Xk = Rk * Math.Cos(Uk);
                textBox33.Text = Xk.ToString();

                //计算卫星在轨道平面坐标Yk
                double Yk = Rk * Math.Sin(Uk);
                textBox34.Text = Yk.ToString();

                //计算卫星在轨道平面坐标Wk
                double we = 7.29211567e-5; //地球自转角速度
                double Wk = omega0 + (omgadot - we) * tk - we * toe;
                textBox35.Text = Wk.ToString();

                //计算卫星在地心固定坐标系的坐标X
                double x = Xk * Math.Cos(Wk) - Yk * Math.Cos(Ik) * Math.Sin(Wk);
                textBox36.Text = x.ToString();

                //计算卫星在地心固定坐标系的坐标Y
                double y = Xk * Math.Sin(Wk) + Yk * Math.Cos(Ik) * Math.Cos(Wk);
                textBox37.Text = y.ToString();

                //计算卫星在地心固定坐标系的坐标Z
                double z = Yk * Math.Sin(Ik);
                textBox38.Text = z.ToString();

            }
            catch (FormatException)
            {
                MessageBox.Show("请输入有效的数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox39_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox41_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
