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
        void DrawMesh(float yaw, float pitch, float roll, float x, float y, float z)
        {
            if (XDevice == null) return;
            XDevice.Transform.World = Matrix.RotationYawPitchRoll(yaw, pitch, roll) * Matrix.Translation(x, y, z);
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
            XDevice.Transform.View = Matrix.LookAtLH(new Vector3(-400, -100, 600.0f), new Vector3(), new Vector3(0, 1, 0));
            XDevice.RenderState.Ambient = Color.Black;
            XDevice.Lights[0].Type = LightType.Directional;
            XDevice.Lights[0].Diffuse = Color.AntiqueWhite;
            XDevice.Lights[0].Direction = new Vector3(0, 1, 0);
            XDevice.Lights[0].Update();
            XDevice.Lights[0].Enabled = true;
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
            
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
                if (XDevice == null) InitializeGraphics();
                
                LoadMesh(openFileDialog_XFile.FileName);
                XDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 1);
                SetCamera();
                XDevice.Present();
                XDevice.BeginScene();
                float angle = 0.0f;
                DrawMesh(angle / (float)Math.PI, angle / (float)Math.PI * 2.0f, angle / (float)Math.PI / 10.0f, 0.0f, -40.0f, 0.0f);
                XDevice.EndScene();

                XDevice.Present();
            }
        }
    }
}
