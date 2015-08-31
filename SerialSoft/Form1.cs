using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialSoft
{
    public partial class Form1 : Form
    {
        private class comboBox2item : Object
        {
            private string m_name = "";
            private int m_value = 0;

            // 表示名称
            public string NAME
            {
                set { m_name = value; }
                get { return m_name; }
            }

            // ボーレート設定値.
            public int BAUDRATE
            {
                set { m_value = value; }
                get { return m_value; }
            }

            // コンボボックス表示用の文字列取得関数.
            public override string ToString()
            {
                return m_name;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //! シリアルポートをオープンしている場合、クローズする.
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //! シリアルポートをオープンしていない場合、処理を行わない.
            if (serialPort1.IsOpen == false)
            {
                return;
            }

            try
            {
                //! 受信データを読み込む.
                string data = serialPort1.ReadExisting();

                //! 受信したデータをテキストボックスに書き込む.
                Invoke(new Delegate_RcvDataToTextBox(RcvDataToTextBox), new Object[] { data });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private delegate void Delegate_RcvDataToTextBox(string data);
        private void RcvDataToTextBox(string data)
        {
            //! 受信データをテキストボックスの最後に追記する.
            if (data != null)
            {
                textBox1.AppendText(data);
            }
        }

        

        private void connectButton_Click(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen == true)
            {
                //! シリアルポートをクローズする.
                serialPort1.Close();

                //! ボタンの表示を[切断]から[接続]に変える.
                button1.Text = "接続";
            }
            else
            {
                //! オープンするシリアルポートをコンボボックスから取り出す.
                serialPort1.PortName = comboBox1.SelectedItem.ToString();

                //! ボーレートをコンボボックスから取り出す.
                comboBox2item baud = (comboBox2item)comboBox2.SelectedItem;
                serialPort1.BaudRate = baud.BAUDRATE;

                //! データビットをセットする. (データビット = 8ビット)
                serialPort1.DataBits = 8;

                //! パリティビットをセットする. (パリティビット = なし)
                serialPort1.Parity = Parity.None;

                //! ストップビットをセットする. (ストップビット = 1ビット)
                serialPort1.StopBits = StopBits.One;

                //! フロー制御をコンボボックスから取り出す.
                //! 文字コードをセットする.
                serialPort1.Encoding = Encoding.ASCII;

                try
                {
                    //! シリアルポートをオープンする.
                    serialPort1.Open();

                    //! ボタンの表示を[接続]から[切断]に変える.
                    button1.Text = "切断";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //! 利用可能なシリアルポート名の配列を取得する.
            string[] PortList = SerialPort.GetPortNames();

            comboBox1.Items.Clear();

            //! シリアルポート名をコンボボックスにセットする.
            foreach (string PortName in PortList)
            {
                comboBox1.Items.Add(PortName);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            comboBox2.Items.Clear();

            // ボーレート選択コンボボックスに選択項目をセットする.
            comboBox2item baud;
            baud = new comboBox2item();
            baud.NAME = "4800bps";
            baud.BAUDRATE = 4800;
            comboBox2.Items.Add(baud);

            baud = new comboBox2item();
            baud.NAME = "9600bps";
            baud.BAUDRATE = 9600;
            comboBox2.Items.Add(baud);

            baud = new comboBox2item();
            baud.NAME = "19200bps";
            baud.BAUDRATE = 19200;
            comboBox2.Items.Add(baud);

            baud = new comboBox2item();
            baud.NAME = "115200bps";
            baud.BAUDRATE = 115200;
            comboBox2.Items.Add(baud);
            comboBox2.SelectedIndex = 1;
            

            // フロー制御選択コンボボックスに選択項目をセットする.

            // 送受信用のテキストボックスをクリアする.
            textBox1.Clear();
        }
    }




}
