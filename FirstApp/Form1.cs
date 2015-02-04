using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstApp
{
    public partial class Form1 : Form
    {
        uint TimerCtr = 0;
        uint TotalTime = 0;
        uint READINGSPERPAGE = 170;
        uint READINGS = 6;
        Thread t;
        ManualResetEvent runThread = new ManualResetEvent(false);
        
        uint NumReceived = 0;
        ushort NumToReceive = 0;
        uint NumToRXInt = 0;
        string DumpFileName;
        DateTime DTRightNow;
        TimeSpan ETALeft;
        string SaveDefaultDirectory;
        /* Create a Stream to write to File */
        BinaryWriter b;
        byte[] Buffer = new byte[4];

        bool Command = false;

        uint PBValue = 0;

        /* Create a serial port */
        SerialPort ComPort1 = new SerialPort("COM5", 115200);


        private void ReceiveThread()
        {
            Debug.WriteLine("Debug Thread (Re)Started");
            while (true)
            {
                runThread.WaitOne(Timeout.Infinite);

                while (true)
                {
                    try
                    {
                        if(Command)
                        { 
                        // receive data 
                            Buffer[2] = (byte)ComPort1.ReadByte();
                            Buffer[3] = (byte)ComPort1.ReadByte();
                            Buffer[0] = (byte)ComPort1.ReadByte();
                            Buffer[1] = (byte)ComPort1.ReadByte();

                            if((Buffer[2] != 'n') || (Buffer[3] != 'c'))
                            {
                                ComPort1.Close();
                                b.Close();
                                lblStat.Parent.Invoke((MethodInvoker)delegate { lblStat.Text = "\'nc\' NOT received. File and Stream Closed"; lblStat.ForeColor = Color.Red; });
                                return;
                            }
                        NumToReceive = (ushort)(((ushort)Buffer[1] << 8) + (ushort)Buffer[0]);
                        NumToRXInt = ((uint)NumToReceive * (READINGSPERPAGE) * READINGS);
                        //lblToRX.Parent.Invoke((MethodInvoker)delegate { lblToRX.Text = NumToRXInt.ToString(); });
                        

                        Command = false;
                        runThread.Reset();
                        }
                        else
                        {
                            b.Write((byte)ComPort1.ReadByte());

                            TimerCtr = 0;
                            NumReceived++;

                            PBValue = (uint)(((double)NumReceived / (double)NumToRXInt) * (double)100);
                            
                            //progressBar1.Parent.Invoke((MethodInvoker)delegate{ progressBar1.Value = (int) PBValue; });

                            //lblRX.Parent.Invoke((MethodInvoker)delegate
                            //{
                            //    lblRX.Text = NumReceived.ToString();

                            //});


                            if(NumReceived > (NumToRXInt-1) )
                            {
                                try
                                {
                                    //Debug.WriteLine(TotalTime.ToString());
                                    ComPort1.Close();
                                    b.Close();
                                    lblStat.Parent.Invoke((MethodInvoker)delegate {
                                                lblStat.Text = "Data Finished, File and Stream Closed";
                                                lblStat.ForeColor = Color.Green; 
                                    
                                    });
                                    
                                    //lblStat.Parent.Invoke((MethodInvoker)delegate { lblETA.Text = TotalTime.ToString(); });
                                    
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                        }
                        /* Store data */
                        //                        this.Invoke(this.m_DelegateAddToList, new Object[] { "R: " + msg });
                    }

                    catch(Exception ex)
                    {
                        Debug.WriteLine("Caught " + ex.Message);
                        runThread.Reset();
                        break;
                    }

                    

                }
            }
        }


        public Form1()
        {
            InitializeComponent();


            t = new Thread(ReceiveThread);
            t.Start();
            
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimerCtr++;
            TotalTime++;
            lblRX.Text = NumReceived.ToString();
            progressBar1.Value = (int)PBValue;
            
            lblTXPercent.Text = PBValue.ToString()+"%";
            lblToRX.Text = NumToRXInt.ToString();
            
            ETALeft = new TimeSpan( ((NumToRXInt - NumReceived) / 14400)*TimeSpan.TicksPerSecond);
            lblETA.Text = ETALeft.ToString();
            
            /* If NO data received */
            if (TimerCtr>30 && ComPort1.IsOpen)
            {
                lblStat.Parent.Invoke((MethodInvoker)delegate { lblStat.Text = "RX error."; lblStat.ForeColor = Color.Red; });
                ModifyProgressBarColor.SetState(progressBar1, 2); // 2 = red
                try { 
                ComPort1.Close();
                
                }
                catch
                {

                }

                b.Close();
            }

        }



        private void BtnDump_Click(object sender, EventArgs e)
        {

            try
                {
                    ComPort1.Open();
                    TimerCtr = 0;
                    TotalTime = 0;
                    timer1.Enabled = true;
                    ComPort1.DiscardInBuffer();
                    ETALeft = new TimeSpan(0);
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            /* Get the current Data and Time to use for filename */
            DTRightNow = DateTime.Now;

            DumpFileName = DTRightNow.Year.ToString() + "_" + DTRightNow.Month.ToString() + "_" + DTRightNow.Day.ToString() + "T" + DTRightNow.Hour.ToString() + "_" + DTRightNow.Minute.ToString() + "_" + DTRightNow.Second.ToString() + ".wrdt";

            SaveDefaultDirectory = "C:\\Users\\Surya\\Dropbox\\Education\\BiteCounter\\WristData Dump\\NewAppDump\\";

        
            lblFileName.Text = DumpFileName;
            /* Write to the file */


            b = new BinaryWriter(File.Open(SaveDefaultDirectory + DumpFileName, FileMode.Create));
            ModifyProgressBarColor.SetState(progressBar1, 1); // 1 = Green
            
            /* Open the Serial Port */
            NumReceived = 0;
            NumToReceive = 0;

            /* The "Send Data" sd command */
            ComPort1.Write("sd");
            Command = true;
            runThread.Set();
            lblStat.Parent.Invoke((MethodInvoker)delegate { lblStat.Text = "Command Sent"; lblStat.ForeColor = Color.Green; });


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //b.Close();
            try
            {
                
                ComPort1.Close();
                b.Close();
                t.Abort();
                t = new Thread(ReceiveThread);
                t.Start();
                lblStat.Text = "File and Stream Closed \n";
                lblStat.ForeColor = Color.YellowGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void lblFileName_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start((SaveDefaultDirectory + DumpFileName));
        }


    }
}


/* Change Progress Bar Color */

public static class ModifyProgressBarColor
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
    public static void SetState(this ProgressBar pBar, int state)
    {
        SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
    }
}