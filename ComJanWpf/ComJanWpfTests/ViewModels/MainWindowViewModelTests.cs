using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComJanWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ComJanWpf.ViewModels.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        [TestMethod()]
        public void GetRandomPointTest()
        {
            ComJanWpf.ViewModels.MainWindowViewModel vm = new MainWindowViewModel();

            // 左上
            {
                System.Drawing.Point p = new System.Drawing.Point(0, 0);
                var pp = vm.GetRandomPoint(p, 30.0);
                if (p.X <= pp.X && p.Y <= pp.Y)
                {
                    // ok
                }
                else
                {
                    Assert.Fail();
                }
            }

            // 右上
            {
                System.Drawing.Point p = new System.Drawing.Point(100, 0);
                var pp = vm.GetRandomPoint(p, 30.0);
                if (p.X >= pp.X && p.Y <= pp.Y)
                {
                    // ok
                }
                else
                {
                    Assert.Fail();
                }
            }

            // 右下
            {
                System.Drawing.Point p = new System.Drawing.Point(100, 100);
                var pp = vm.GetRandomPoint(p, 30.0);
                if (p.X >= pp.X && p.Y >= pp.Y)
                {
                    // ok
                }
                else
                {
                    Assert.Fail();
                }
            }

            // 左下
            {
                System.Drawing.Point p = new System.Drawing.Point(0, 100);
                var pp = vm.GetRandomPoint(p, 30.0);
                if (p.X <= pp.X && p.Y >= pp.Y)
                {
                    // ok
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void CreateStudyDataTest()
        {
            Bitmap bmp = new Bitmap(@"C:\Users\seiyu\Documents\Visual Studio 2015\Projects\ComJan\bin\Debug\j1ton.bmp");
            ComJanWpf.ViewModels.MainWindowViewModel vm = new MainWindowViewModel();

            for(int i = 0; i < 100; i++)
            {
                Bitmap bb = vm.CreateStudyData(bmp);
                bb.Save(@"C:\Users\seiyu\Documents\ComJanData\temp\aff_" + (i+1).ToString() + ".bmp");
            }
            

            //Assert.Fail();
        }
    }
}