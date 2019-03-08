using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GradesAssignment
{
    public partial class frmWaiting : Form
    {
        public const int TimerIntervalInMs = 300;

        private static int NextWndID = 1;
        /// <summary>
        /// Key - id окна
        /// </summary>
        private readonly static Dictionary<int, frmWaiting> m_dictAllWnds = new Dictionary<int, frmWaiting>();

        public int ID { get; }

        public bool AllowClose { get; set; } = false;
        
        int m_RemTimersCountForShow = 0;
        Point m_OldTL = new Point(0, 0);
        
        System.Windows.Forms.Timer m_tmrSearching = new System.Windows.Forms.Timer() { Interval = TimerIntervalInMs };


        public void ClearOwner()
        {
            Owner = null;
        }

        public frmWaiting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="ShowingPauseInMs">
        /// Через сколько милисекунд отобразиться окно. Нужно задавать значения кратные TimerIntervalInMs
        /// </param>
        public frmWaiting(Form owner, int ShowingPauseInMs = 0)
        {
            InitializeComponent();

            if (frmMain.Instance != null)
                frmMain.Instance.WaitingForm = this;

            TopMost = true;

            ID = NextWndID++;

            Owner = owner;

            m_OldTL = new Point(Owner.Location.X + Owner.Size.Width / 2 - Size.Width / 2,
                                Owner.Location.Y + Owner.Size.Height / 2 - Size.Height / 2);
            Location = new Point(10000, 10000);

            m_tmrSearching.Tick += (s, ev) =>
            {
                if (m_RemTimersCountForShow-- == 0)
                {
                    Location = m_OldTL;
                    try
                    {
                        Show();
                    }
                    catch
                    { }
                }
            };
            m_RemTimersCountForShow = ShowingPauseInMs / (int)Math.Max(1, m_tmrSearching.Interval);
            m_tmrSearching.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !AllowClose;

            if (!e.Cancel)
            {
                m_tmrSearching.Stop();

                m_dictAllWnds.Remove(ID);
            }

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (Owner == null || Owner.Handle == IntPtr.Zero)
                frmMain.Instance.Activate();
            else if (Owner != frmMain.Instance || Owner.Handle == IntPtr.Zero)
            {
                if (Owner.Handle == IntPtr.Zero)
                    frmMain.Instance.Activate();
                else
                    Owner.Activate();
            }

            if (frmMain.Instance != null)
                frmMain.Instance.WaitingForm = null;

            base.OnClosed(e);
        }
    }
}
