﻿
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LaserGRBL.UserControls
{


	public class WaitingProgressBar : ColorProgressBar
	{

		#region " Codice generato da Progettazione Windows Form "

		public WaitingProgressBar() : base()
		{

			//Chiamata richiesta da Progettazione Windows Form.
			InitializeComponent();

			//Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
		}

		//UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				Stop();
				TIM.Close();

				/*if ((components != null)) {
					components.Dispose();
				}*/
			}
			base.Dispose(disposing);
		}

		//Richiesto da Progettazione Windows Form

		//private IContainer components;
		//NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
		//Può essere modificata in Progettazione Windows Form.  
		//Non modificarla nell'editor del codice.
		private System.Timers.Timer withEventsField_TIM;
		internal System.Timers.Timer TIM {
			get { return withEventsField_TIM; }
			set {
				if (withEventsField_TIM != null) {
					withEventsField_TIM.Elapsed -= TIM_Elapsed;
				}
				withEventsField_TIM = value;
				if (withEventsField_TIM != null) {
					withEventsField_TIM.Elapsed += TIM_Elapsed;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.TIM = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)this.TIM).BeginInit();
			//
			//TIM
			//
			this.TIM.SynchronizingObject = this;
			//
			//WaitingProgressBar
			//
			this.Name = "WaitingProgressBar";
			this.Size = new System.Drawing.Size(129, 16);
			((System.ComponentModel.ISupportInitialize)this.TIM).EndInit();

		}

		#endregion

		public bool Running {
			get { return TIM.Enabled; }
			set {
				if (!(value == TIM.Enabled)) {
					if (value) {
						Start();
					} else {
						Stop();
					}
				}
			}
		}

		public double Interval {
			get { return TIM.Interval; }
			set { TIM.Interval = value; }
		}

		public void Start()
		{
			if (!Running) {
				TIM.Enabled = true;
			}
		}

		public void Stop()
		{
			if (Running) {
				TIM.Stop();
				this.Value = 0;
			}
		}

		private void TIM_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (TIM.Enabled) {
				if (this.BouncingMode == BouncingModeEnum.RollingStrip) {
					if (this.Value == this.Maximum) {
						this.Value = 0;
					} else {
						this.PerformStep();
					}
				} else {
					if (this.Reverse) {
						this.PerformStepBack();
					} else {
						this.PerformStep();
					}

					if (this.Value == this.Maximum | this.Value == this.Minimum)
						this.Reverse = !this.Reverse;
				}
			}
		}

		public enum BouncingModeEnum
		{
			PingPong,
			FilledUpDown,
			RollingStrip
		}

		private BouncingModeEnum _BouncingMode = BouncingModeEnum.PingPong;
		public BouncingModeEnum BouncingMode {
			get { return _BouncingMode; }
			set { _BouncingMode = value; }
		}


		protected override void DrawProgres(System.Drawing.Graphics g)
		{

			try {
				if (this.BouncingMode == BouncingModeEnum.FilledUpDown) {
					base.DrawProgres(g);
				} else {
					//SALVA HEIGHT E WIDTH PER CALCOLI PIU VELOCI
					int W = this.ClientRectangle.Width;
					int H = this.ClientRectangle.Height;

					int BarWidth = 0;
					int BarHeight = 0;
					int BarPosition = 0;
					Rectangle ColoredBar = default(Rectangle);

					BarHeight = H - 4;

					if (this.BouncingMode == BouncingModeEnum.RollingStrip) {
						BarWidth = Convert.ToInt32((W - 3) / 3);
						BarPosition = Convert.ToInt32(Math.Floor(((W + BarWidth + 4) * (this.Value - Minimum)) / (Maximum - Minimum)));
						if (!(BarWidth <= 0) & !(BarHeight <= 0)) {
							ColoredBar = new Rectangle(BarPosition - BarWidth, 2, BarWidth, BarHeight);
						}
					}

					if (this.BouncingMode == BouncingModeEnum.PingPong) {
						BarWidth = Convert.ToInt32((W - 3) / 3);
						BarPosition = Convert.ToInt32(Math.Floor(((W + BarWidth + 4) * (this.Value - Minimum)) / (Maximum - Minimum)));
						if (!(BarWidth <= 0) & !(BarHeight <= 0)) {
							if (Reverse) {
								ColoredBar = new Rectangle(BarPosition - BarWidth - 1, 2, BarWidth, BarHeight);
							} else {
								ColoredBar = new Rectangle(BarPosition - BarWidth, 2, BarWidth, BarHeight);
							}
						}

					}

					if (!ColoredBar.Equals(Rectangle.Empty)) {
						using (LinearGradientBrush brush = new LinearGradientBrush(ColoredBar, this.FillColor, this.BarColor, 90f)) {
							float[] relativeIntensities = {
								0.1f,
								1f,
								1f,
								1f,
								1f,
								0.85f,
								0.1f
							};
							float[] relativePositions = {
								0f,
								0.2f,
								0.5f,
								0.5f,
								0.5f,
								0.8f,
								1f
							};

							Blend blend = new Blend();
							blend.Factors = relativeIntensities;
							blend.Positions = relativePositions;
							brush.Blend = blend;

							g.FillRectangle(brush, ColoredBar);
						}
					}

				}
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			}

		}
	}



}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
