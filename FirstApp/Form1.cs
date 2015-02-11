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
        /* Timer and Background Variables */
        uint TimerCtr = 0;
        uint TotalTime = 0;
        Thread t;
        ManualResetEvent runThread = new ManualResetEvent(false);
        DateTime DTRightNow;
        DateTime Genesis = new DateTime(2015, 1, 1);
        


        /* Variables for Serial Data Read Setup */
        uint READINGSPERPAGE = 170;                         // Number of readings per page. For AT45DB642D this is 170.
        uint READINGS = 6;                                  // Number of Sensors. This is 6.
        ushort NumToReceive = 0;                            // Number that the device sends to us.
        uint NumToRXInt = 0;                                // Calculated Number of bytes that we have to RX = 170 * 6 * NumReceived
        string DumpFileName;                                // Name of file where data is dropped. This is generated from current time.
        string SaveDefaultDirectory;                        // Default Directory where file is dropped. Used for dialog box.
        bool Command = false;                               // 1 after a "Send Data" command is sent to device, then reset to 0.
        SerialPort ComPort1 = new SerialPort("COM5", 115200); // Serial Port for communication.

        /* Variables for Serial Data Read Progress */
        uint NumReceived = 0;                               // Actual Number of bytes RX at time t.
        TimeSpan ETALeft;                                   // Calculated time left for transfer to complete.
        uint PBValue = 0;                                   // Progress bar percentage. Needs to be int.


        /* Variables for Syncting Time with device */
        byte[] TimeStamp = new byte[4];                     // TimeStamp buffer to send to device (or receive).
        TimeSpan TimeStampSpan;                             // Used to calculate time between Jan 1,2015 and right now.
        UInt32 TimeStamp32;                                 // 32 bit variable to store the seconds between now and Jan 1,2015

        
        /* Create a Stream to write to File */
        BinaryWriter b;                                     // Object to write to a file.
        byte[] Buffer = new byte[4];                        // Receives metadata from device.

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
                        NumToRXInt = ((uint)NumToReceive * (READINGSPERPAGE) * READINGS) + 12; // 12 is the number of chars in START\n and ENDDAT
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
                            
                            


                            if(NumReceived > (NumToRXInt-1) )
                            {
                                try
                                {
                                   
                                    ComPort1.Close();
                                    b.Close();
                                    lblStat.Parent.Invoke((MethodInvoker)delegate {
                                                lblStat.Text = "Data Finished, File and Stream Closed";
                                                lblStat.ForeColor = Color.Green; 
                                    
                                    });
                                    
                                 
                                    
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                        }
                       
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

        private void SyncReceiveThread()
    {


    }

        public Form1()
        {
            InitializeComponent();

            CmbPorts.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                CmbPorts.Items.Add(port);
            }
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
            
            ComPort1.PortName = CmbPorts.Text;
            t = new Thread(ReceiveThread);
            t.Start();
            
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
            Thread.Sleep(200);
            ComPort1.Write("ab");
            Thread.Sleep(200);
            ComPort1.Write("cd");
            Command = true;
            runThread.Set();
            lblStat.Parent.Invoke((MethodInvoker)delegate { lblStat.Text = "Command Sent"; lblStat.ForeColor = Color.Green; });


            
        }

        private void lblFileName_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start((SaveDefaultDirectory + DumpFileName));
        }

        private void BtnListPorts_Click(object sender, EventArgs e)
        {
            CmbPorts.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                CmbPorts.Items.Add(port);
            }
        }

        private void btnStopDump(object sender, EventArgs e)
        {
            try
            {
                t.Abort();
                ComPort1.Close();
                b.Close();


                lblStat.Text = "File and Stream Closed \n";
                lblStat.ForeColor = Color.YellowGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnTimeSync_Click(object sender, EventArgs e)
        {
            ComPort1.PortName = CmbPorts.Text;
            t = new Thread(ReceiveThread);
            t.Start();


            try
            {
                ComPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            /* Get the current Data and Time to use to sync */
            DTRightNow = DateTime.Now;
            TimeStampSpan = DTRightNow - Genesis;
            TimeStamp32 = (UInt32)TimeStampSpan.TotalSeconds;
            TimeStamp[0] = (byte)(TimeStamp32 & 0x000000FF);
            TimeStamp[1] = (byte)((TimeStamp32 >> 8) & 0x000000FF);
            TimeStamp[2] = (byte)((TimeStamp32 >> 16) & 0x000000FF);
            TimeStamp[3] = (byte)((TimeStamp32 >> 24) & 0x000000FF);
            
            
            /* The Time synC "TC" command */
            ComPort1.Write("TC");
            ComPort1.Write(TimeStamp, 0, 4);


            lblStat.Text = "Time Sent " + TimeStamp32.ToString() ; lblStat.ForeColor = Color.Green;
            lblTXPercent.Text = TimeStamp32.ToString("X");
            try { ComPort1.Close(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            t.Abort();
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