using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectX9Demo
{
    public partial class Form1 : Form
    {
        #region DirectX9
        Mesh XObjectMesh = null;// X文件网格
        Device XDevice = null;// X显示设备
        Material[] XObjectMeshMaterials;// X文件材质
        Texture[] XObjectMeshTextures;// X文件纹理
        string XObjectFilePath;
        float ViewPositionX = -400;// 摄像机X轴位置
        float ViewPositionY = -100;// 摄像机Y轴位置
        float ViewPositionZ = 600.0f;// 摄像机Z轴位置

        float ViewAngleX = 0;
        float ViewAngleY = 0;
        float ViewAngleZ = 0;

        float MouseXold;
        float MouseYold;

        float XScale = 1;// 模型缩放

        /// <summary>
        /// 初始化图形设备
        /// </summary>
        void InitializeGraphics()
        {
            PresentParameters XPresentParam = new PresentParameters();
            XPresentParam.Windowed = true;// 窗口模式运行
            // 设置交换效果
            XPresentParam.SwapEffect = SwapEffect.Discard;
            XPresentParam.AutoDepthStencilFormat = DepthFormat.D16;
            XPresentParam.EnableAutoDepthStencil = true;
            // 创建设备
            XDevice = new Device(0, DeviceType.Hardware, panel_Disp, CreateFlags.SoftwareVertexProcessing, XPresentParam);

        }
        /// <summary>
        /// 加载网格文件
        /// </summary>
        /// <param name="filePath"></param>
        void LoadMesh(string filePath)
        {
            if (XDevice == null) return;
            ExtendedMaterial[] XExtMaterials = null;
            try
            {
                XObjectMesh = Mesh.FromFile(filePath, MeshFlags.Managed, XDevice, out XExtMaterials);
                // 如果有材质就载入
                if (XExtMaterials != null && XExtMaterials.Length > 0)
                {
                    // 加载材质和纹理
                    XObjectMeshMaterials = new Material[XExtMaterials.Length];
                    XObjectMeshTextures = new Texture[XExtMaterials.Length];
                    for (int i = 0; i < XExtMaterials.Length; i++)
                    {
                        XObjectMeshMaterials[i] = XExtMaterials[i].Material3D;
                        if (XExtMaterials[i].TextureFilename != null && XExtMaterials[i].TextureFilename != string.Empty)
                        {
                            XObjectMeshTextures[i] = TextureLoader.FromFile(XDevice, filePath + XExtMaterials[i].TextureFilename);
                        }
                    }
                }
            }
            catch (Direct3DXException D3DXe)
            {
                MessageBox.Show(D3DXe.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 绘制模型
        /// </summary>
        /// <param name="yaw"></param>
        /// <param name="pitch"></param>
        /// <param name="roll"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void DrawMesh()
        {
            if (XDevice == null) return;
            XDevice.Transform.World = Matrix.RotationYawPitchRoll(ViewAngleX, ViewAngleY, ViewAngleZ) * Matrix.Translation(0, -40.0f, 0) * Matrix.Scaling(XScale, XScale, XScale); ;
            
            // 绘制
            for (int i = 0; i < XObjectMeshMaterials.Length; i++)
            {
                XDevice.Material = XObjectMeshMaterials[i];
                XDevice.SetTexture(0, XObjectMeshTextures[i]);
                XObjectMesh.DrawSubset(i);
            }
        }
        /// <summary>
        /// 设置视角与灯光
        /// </summary>
        void SetCamera()
        {
            if (XDevice == null) return;
            XDevice.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 12, this.Width / this.Height, 0.80f, 10000.0f);
            XDevice.Transform.View = Matrix.LookAtLH(new Vector3(ViewPositionX, ViewPositionY, ViewPositionZ),
                new Vector3(), new Vector3(0, 1, 0));
            XDevice.RenderState.Ambient = Color.Black;
            XDevice.Lights[0].Type = LightType.Directional;
            XDevice.Lights[0].Diffuse = Color.AntiqueWhite;
            XDevice.Lights[0].Specular = Color.White;
            XDevice.Lights[0].Direction = new Vector3(1, -1, 0);
            XDevice.Lights[0].Update();
            XDevice.Lights[0].Enabled = true;
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
            MouseXold = panel_Disp.Width / 2;
            MouseYold = panel_Disp.Height / 2;
            panel_Disp.MouseWheel += new MouseEventHandler(Panel_MouseWheelEvent);
            panel_Disp.Width = this.Width - panel_Disp.Location.X - 40;
            panel_Disp.Height = this.Height - panel_Disp.Location.Y - 60;
        }
        /// <summary>
        /// 菜单项点击处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void MenuItemClickEventHandler(object sender, EventArgs args)
        {
            if (sender.Equals(toolStripMenuItem_File_Open)) menuItem_OpenFile_Click();
        }
        void menuItem_OpenFile_Click()
        {
            if (openFileDialog_XFile.ShowDialog() == DialogResult.OK)
            {
                XScale = 1;
                
                if (XDevice == null) InitializeGraphics();
                XObjectFilePath = openFileDialog_XFile.FileName;
                LoadMesh(XObjectFilePath);
                XDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 1);
                SetCamera();
                XDevice.Present();
                XDevice.BeginScene();
                DrawMesh();
                XDevice.EndScene();

                XDevice.Present();
            }
        }
        /// <summary>
        /// 鼠标拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Math.Abs(e.X - MouseXold) < 50 && Math.Abs(e.Y - MouseYold) < 50)
                {
                    ViewAngleX -= 0.01f * (e.X - MouseXold);
                    if (ViewAngleX > 0)
                    {
                        if ((ViewAngleX / Math.PI) % 2 == 0)
                        {
                            ViewAngleY += 0.01f * (e.Y - MouseYold);
                        }
                        else
                        {
                            ViewAngleY -= 0.01f * (e.Y - MouseYold);
                        }
                    }
                    else
                    {
                        if ((ViewAngleX / Math.PI) % 2 == 0)
                        {
                            ViewAngleY -= 0.01f * (e.Y - MouseYold);
                        }
                        else
                        {
                            ViewAngleY += 0.01f * (e.Y - MouseYold);
                        }
                    }
                }
                label_ViewAngleX.Text = "ViewAngleX : " + ViewAngleX;
                label_ViewAngleY.Text = "ViewAngleY : " + ViewAngleY;
                Application.DoEvents();
                MouseXold = e.X;
                MouseYold = e.Y;
                Render();
            }
        }
        /// <summary>
        /// 鼠标滚轮事件，用来做缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseWheelEvent(object sender, MouseEventArgs e)
        {
            //MouseDeltaOld = e.Delta;
            XScale += (0.001f * e.Delta);
            if (XScale > 8) XScale = 8;
            if (XScale < 0.2f) XScale = 0.2f;
            Render();
        }
        /// <summary>
        /// 渲染
        /// </summary>
        public void Render()
        {
            if (XDevice == null) return;
            XDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 1);
            SetCamera();
            DrawMesh();
            XDevice.BeginScene();
            XDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            XDevice.EndScene();
            XDevice.Present();
        }
        /// <summary>
        /// 使显示区域与窗体同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            panel_Disp.Width = this.Width - panel_Disp.Location.X - 40;
            panel_Disp.Height = this.Height - panel_Disp.Location.Y - 60;

            if (XDevice == null) InitializeGraphics();
            XObjectFilePath = openFileDialog_XFile.FileName;
            LoadMesh(XObjectFilePath);
            XDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 1);
            SetCamera();
            XDevice.Present();
            XDevice.BeginScene();
            DrawMesh();
            XDevice.EndScene();

            XDevice.Present();
            //Render();
        }
    }
}
