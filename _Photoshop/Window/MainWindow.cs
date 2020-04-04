using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyPhotoshop
{
	public class MainWindow : Form
	{
		Bitmap originalBmp;
		Photo originalPhoto;
		PictureBox original;
		PictureBox processed;
		ComboBox filtersSelect;
		Panel parametersPanel;
		List<NumericUpDown> parametersControls;
		Button apply;
		Button reset;
		Font defaultFont1;
		Font defaultFont2;
		Bitmap bitmap;

		public MainWindow()
		{
			original = new PictureBox();
			Controls.Add(original);

			processed = new PictureBox();
			Controls.Add(processed);

			filtersSelect = new ComboBox();
			filtersSelect.DropDownStyle = ComboBoxStyle.DropDownList;
			filtersSelect.SelectedIndexChanged += FilterChanged;
			Controls.Add(filtersSelect);

			apply = new Button();
			apply.Text = "Применить";
			apply.Enabled = false;
			apply.Click += Process;
			Controls.Add(apply);

			reset = new Button();
			reset.Text = "Сброс";
			reset.Enabled = false;
			reset.Click += Reset;
			Controls.Add(reset);

			defaultFont1 = new Font("Arial", 12);
			defaultFont2 = new Font("Arial", 10);

			Text = "Photoshop pre-alpha release";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;

			bitmap = (Bitmap)Image.FromFile("sorc.jpg");
			LoadBitmap(bitmap);
		}

		public void LoadBitmap(Bitmap bmp)
		{
			originalBmp = bmp;
			originalPhoto = Convertors.Bitmap2Photo(bmp);

			original.Image = originalBmp;
			original.Left = 0;
			original.Top = 0;
			original.ClientSize = originalBmp.Size;

			processed.Left = 0;
			processed.Top = original.Bottom;
			processed.Size = original.Size;

			filtersSelect.Left = original.Right + 10;
			filtersSelect.Top = 20;
			filtersSelect.Width = 250;
			filtersSelect.Height = 20;
			filtersSelect.Font = defaultFont1;

			ClientSize = new Size(filtersSelect.Right + 20, processed.Bottom);

			apply.Width = 150;
			apply.Height = 50;
			apply.Left = ClientSize.Width - apply.Width - 20;
			apply.Top = ClientSize.Height - apply.Height - 10;
			apply.Font = new Font("Arial", 14);

			reset.Width = 150;
			reset.Height = 50;
			reset.Left = ClientSize.Width - reset.Width - 20;
			reset.Top = ClientSize.Height - apply.Height - reset.Height - 20;
			reset.Font = new Font("Arial", 14);

			FilterChanged(null, EventArgs.Empty);
		}


		public void AddFilter(IFilter filter)
		{
			filtersSelect.Items.Add(filter);
			if (filtersSelect.SelectedIndex == -1)
			{
				filtersSelect.SelectedIndex = 0;
				apply.Enabled = true;
			}
		}

		void FilterChanged(object sender, EventArgs e)
		{
			var filter = (IFilter)filtersSelect.SelectedItem;
			if (filter == null) return;
			if (parametersPanel != null) Controls.Remove(parametersPanel);
			parametersControls = new List<NumericUpDown>();
			parametersPanel = new Panel();
			parametersPanel.Left = filtersSelect.Left;
			parametersPanel.Top = filtersSelect.Bottom + 10;
			parametersPanel.Width = filtersSelect.Width;
			parametersPanel.Height = ClientSize.Height - parametersPanel.Top;

			int y = 0;

			foreach (var param in filter.GetParameters())
			{
				var label = new Label();
				label.Left = 0;
				label.Top = y;
				label.Width = parametersPanel.Width - 50;
				label.Height = 20;
				label.Text = param.Name;
				label.Font = defaultFont2;
				parametersPanel.Controls.Add(label);

				var box = new NumericUpDown();
				box.Left = label.Right;
				box.Top = y;
				box.Width = 50;
				box.Height = 20;
				box.Value = (decimal)param.DefaultValue;
				box.Increment = (decimal)param.Increment;
				box.Maximum = (decimal)param.MaxValue;
				box.Minimum = (decimal)param.MinValue;
				box.DecimalPlaces = 2;
				label.Font = defaultFont2;
				parametersPanel.Controls.Add(box);
				y += label.Height + 5;
				parametersControls.Add(box);
			}
			Controls.Add(parametersPanel);
		}


		void Process(object sender, EventArgs empty)
		{
			var data = parametersControls.Select(z => (double)z.Value).ToArray();
			var filter = (IFilter)filtersSelect.SelectedItem;
			Photo result = null;
			result = filter.Process(originalPhoto, data);
			var resultBmp = Convertors.Photo2Bitmap(result);
			if (resultBmp.Width > originalBmp.Width || resultBmp.Height > originalBmp.Height)
			{
				float k = Math.Min((float)originalBmp.Width / resultBmp.Width, (float)originalBmp.Height / resultBmp.Height);
				var newBmp = new Bitmap((int)(resultBmp.Width * k), (int)(resultBmp.Height * k));
				using (var g = Graphics.FromImage(newBmp))
				{
					g.DrawImage(resultBmp, new Rectangle(0, 0, newBmp.Width, newBmp.Height), new Rectangle(0, 0, resultBmp.Width, resultBmp.Height), GraphicsUnit.Pixel);
				}
				resultBmp = newBmp;
			}

			processed.Image = resultBmp;
			reset.Enabled = true;
			originalPhoto = Convertors.Bitmap2Photo(resultBmp);
		}

		private void Reset(object sender, EventArgs e)
		{
			reset.Enabled = false;
			originalPhoto = Convertors.Bitmap2Photo(bitmap);

			filtersSelect.SelectedIndex = 0;

			FilterChanged(null, EventArgs.Empty);
			Process(null, EventArgs.Empty);
		}
	}
}