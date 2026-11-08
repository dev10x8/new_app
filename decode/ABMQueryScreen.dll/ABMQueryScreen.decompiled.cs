using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ABMQueryScreen.Bl;
using CrystalDecisions.CrystalReports.Engine;

[assembly: AssemblyCopyright("Copyright ABM Technomatrix (P) Ltd. ©  2016")]
[assembly: AssemblyTrademark("ABM Technomatrix (P) Ltd.")]
[assembly: ComVisible(false)]
[assembly: AssemblyProduct("ABM Query Screen")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyConfiguration("")]
[assembly: ThemeInfo(/*Could not decode attribute arguments.*/)]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyCompany("ABM Technomatrix (P) Ltd.")]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyTitle("ABM Query Screen")]
[assembly: AssemblyDescription("ABM Query Screen")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace ABMQueryScreen.Bl
{
	internal class ClsFilterShowData
	{
		private List<string> gridColumnName = new List<string>();

		private List<string> gridColumnType = new List<string>();

		private object gridName = new object();

		private object gridTotal = new object();

		private string top;

		private string left;

		private string height;

		private string width;

		public List<int> filterIndex = new List<int>();

		private DataTable dt = new DataTable();

		private bool isFilterApplicable;

		public List<string> GetSetGridColumnName
		{
			get
			{
				return gridColumnName;
			}
			set
			{
				gridColumnName = value;
			}
		}

		public List<string> GetSetGirdColumnType
		{
			get
			{
				return gridColumnType;
			}
			set
			{
				gridColumnType = value;
			}
		}

		public object GetSetGridName
		{
			get
			{
				return gridName;
			}
			set
			{
				gridName = value;
			}
		}

		public object GetSetGridTotal
		{
			get
			{
				return gridTotal;
			}
			set
			{
				gridTotal = value;
			}
		}

		public DataTable BindingDataTable { get; set; }

		public string QueryString { get; set; }

		public bool IsFiltarable { get; set; }

		public string columnWidth { get; set; }

		public string connectionString { get; set; }

		public ClsFilterShowData(List<string> columnNameCurrent, List<string> columnTypeCurrent)
		{
			gridColumnName = columnNameCurrent;
			gridColumnType = columnTypeCurrent;
		}

		public void BindDataTable(int grdno, bool CheckboxVisible)
		{
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Expected O, but got Unknown
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			if (isFilterApplicable)
			{
				if (BindingDataTable != null)
				{
					DataGrid val = (DataGrid)gridName;
					DataGridCheckBoxColumn val2 = new DataGridCheckBoxColumn();
					string text = BindingDataTable.Columns[0].ToString();
					((DataGridBoundColumn)val2).Binding = (BindingBase)new Binding(BindingDataTable.Columns[BindingDataTable.Columns.Count - 1].ToString());
					val.Columns.Add((DataGridColumn)(object)val2);
					List<string> list = gridColumnName;
					List<string> list2 = gridColumnType;
					for (int i = 0; i < list.Count; i++)
					{
						DataGridTextColumn val3 = new DataGridTextColumn();
						Style val4 = new Style(typeof(TextBlock));
						((Collection<SetterBase>)(object)val4.Setters).Add((SetterBase)new Setter(TextBlock.TextWrappingProperty, (object)(TextWrapping)2));
						((DataGridBoundColumn)val3).ElementStyle = val4;
						((DataGridColumn)val3).Header = list[i].ToString();
						((DataGridBoundColumn)val3).Binding = (BindingBase)new Binding(list[i].ToString());
						val.Columns.Add((DataGridColumn)(object)val3);
						((DataGridColumn)val3).IsReadOnly = true;
					}
					((ItemsControl)val).ItemsSource = BindingDataTable.DefaultView;
					((CollectionView)((ItemsControl)val).Items).Refresh();
					if (!CheckboxVisible)
					{
						val.Columns[0].Visibility = (Visibility)1;
					}
					else
					{
						val.Columns[0].Visibility = (Visibility)0;
					}
					if (BindingDataTable.Columns.Count > 1)
					{
						LoadTotalAmount(grdno + 1);
					}
					gridName = val;
				}
			}
			else
			{
				DataGrid val = (DataGrid)gridName;
				DataGridCheckBoxColumn val2 = new DataGridCheckBoxColumn();
				val.Columns.Clear();
				for (int i = 0; i < gridColumnName.Count; i++)
				{
					DataGridTextColumn val3 = new DataGridTextColumn();
					Style val4 = new Style(typeof(TextBlock));
					((Collection<SetterBase>)(object)val4.Setters).Add((SetterBase)new Setter(TextBlock.TextWrappingProperty, (object)(TextWrapping)2));
					((DataGridBoundColumn)val3).ElementStyle = val4;
					((DataGridColumn)val3).Header = gridColumnName[i].ToString();
					((DataGridBoundColumn)val3).Binding = (BindingBase)new Binding(gridColumnName[i].ToString());
					val.Columns.Add((DataGridColumn)(object)val3);
					((DataGridColumn)val3).IsReadOnly = true;
				}
				((ItemsControl)val).ItemsSource = dt.DefaultView;
				((CollectionView)((ItemsControl)val).Items).Refresh();
				if (!CheckboxVisible)
				{
					val.Columns[0].Visibility = (Visibility)1;
				}
				else
				{
					val.Columns[0].Visibility = (Visibility)0;
				}
				gridName = val;
			}
		}

		public void AdjustColumnWidth()
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			List<string> list = columnWidth.Split(new char[1] { ',' }).ToList();
			if (list.Count() == 1)
			{
				try
				{
					for (int i = 0; i < ((DataGrid)GetSetGridName).Columns.Count(); i++)
					{
						((DataGrid)GetSetGridName).Columns[i].MinWidth = double.Parse(list[i]);
						((DataGrid)GetSetGridName).Columns[i].MaxWidth = double.Parse(list[i]);
					}
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			for (int i = 0; i < list.Count(); i++)
			{
				try
				{
					((DataGrid)GetSetGridName).Columns[i].MinWidth = double.Parse(list[i]);
					((DataGrid)GetSetGridName).Columns[i].MaxWidth = double.Parse(list[i]);
				}
				catch (IndexOutOfRangeException)
				{
					break;
				}
				catch (InvalidCastException)
				{
				}
				catch (Exception)
				{
				}
			}
		}

		public void LoadTotalAmount(int grdno)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Expected O, but got Unknown
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Expected O, but got Unknown
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			if (!(((FrameworkElement)(Control)GetSetGridName).Name != "StockSummary"))
			{
				return;
			}
			DataGrid val = (DataGrid)gridTotal;
			val.Columns.Clear();
			string text = ((FrameworkElement)val).Name.ToString();
			List<string> list = gridColumnName;
			List<string> list2 = gridColumnType;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			string text2 = "";
			for (int i = 0; i < BindingDataTable.Rows.Count; i++)
			{
				text2 = BindingDataTable.Rows[i][0].ToString();
				if (BindingDataTable.TableName == "KFLList" || BindingDataTable.TableName == "KFLWiseTotal")
				{
					if (text2 == "CS")
					{
						num2 += Convert.ToDouble(BindingDataTable.Rows[i][1].ToString());
					}
					else
					{
						try
						{
							num += Convert.ToDouble(BindingDataTable.Rows[i][1].ToString());
						}
						catch (Exception)
						{
						}
					}
				}
				else
				{
					try
					{
						num += Convert.ToDouble(BindingDataTable.Rows[i][1].ToString());
					}
					catch (Exception)
					{
					}
				}
				if (BindingDataTable.Rows[i][2].GetType().ToString() != "System.Boolean")
				{
					if (BindingDataTable.TableName == "KFLList" || BindingDataTable.TableName == "KFLWiseTotal")
					{
						if (text2 == "CS")
						{
							num4 += Convert.ToDouble(BindingDataTable.Rows[i][2].ToString());
						}
						else
						{
							num3 += Convert.ToDouble(BindingDataTable.Rows[i][2].ToString());
						}
					}
					else
					{
						num3 += Convert.ToDouble(BindingDataTable.Rows[i][2].ToString());
					}
					num5 = ((!(BindingDataTable.Rows[i][3].GetType().ToString() != "System.Boolean")) ? 0.0 : (num5 + Convert.ToDouble(BindingDataTable.Rows[i][3].ToString())));
				}
				else
				{
					num3 = 0.0;
				}
			}
			DataTable dataTable = new DataTable();
			dataTable.TableName = BindingDataTable.TableName;
			if (num2 > 0.0)
			{
				dataTable.Columns.Add("TotalCs", typeof(string));
				for (int i = 0; i < gridColumnName.Count - 1; i++)
				{
					dataTable.Columns.Add(gridColumnName[i + 1].ToString() + "1", gridColumnType[i + 1].GetType());
				}
			}
			dataTable.Columns.Add("Total", typeof(string));
			for (int i = 0; i < gridColumnName.Count - 1; i++)
			{
				dataTable.Columns.Add(gridColumnName[i + 1].ToString(), gridColumnType[i + 1].GetType());
			}
			DataRow dataRow = dataTable.NewRow();
			if (num2 > 0.0)
			{
				dataRow["TotalCs"] = "TotCs";
				dataRow[1] = Math.Round(num2, 2);
				if (num4 > 0.0)
				{
					dataRow[2] = Math.Round(num4, 2);
				}
				dataRow["Total"] = "TotOthr";
			}
			else
			{
				dataRow["Total"] = "Total";
			}
			dataRow[gridColumnName[1]] = Math.Round(num, 2);
			if (num3 != 0.0)
			{
				dataRow[gridColumnName[2]] = Math.Round(num3, 2);
			}
			if (num5 != 0.0)
			{
				dataRow[gridColumnName[3]] = Math.Round(num5, 2);
			}
			dataTable.Rows.Add(dataRow);
			val.Columns.Clear();
			((FrameworkElement)val).DataContext = dataTable;
			for (int j = 0; j < dataTable.Columns.Count; j++)
			{
				DataGridTextColumn val2 = new DataGridTextColumn();
				((DataGridColumn)val2).Header = dataTable.Columns[j].ToString();
				((DataGridBoundColumn)val2).Binding = (BindingBase)new Binding(dataTable.Columns[j].ToString());
				val.Columns.Add((DataGridColumn)(object)val2);
				((DataGridColumn)val2).IsReadOnly = true;
			}
			((ItemsControl)val).ItemsSource = dataTable.DefaultView;
			for (int j = 0; j < val.Columns.Count; j++)
			{
				val.Columns[j].Width = DataGridLength.op_Implicit(50.0);
			}
			((Control)val).FontSize = 11.0;
			((CollectionView)((ItemsControl)val).Items).Refresh();
			gridTotal = val;
		}

		private void CreateDataTable()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			BindingDataTable = new DataTable(((FrameworkElement)(DataGrid)gridName).Name);
			for (int i = 0; i < gridColumnName.Count; i++)
			{
				string text = gridColumnType[i].GetType().ToString();
				Type type = Type.GetType(GetType("String"));
				DataColumn column = new DataColumn(gridColumnName[i].ToString(), Type.GetType(GetType(gridColumnType[i])));
				BindingDataTable.Columns.Add(column);
			}
			BindingDataTable.Columns.Add("IsSelected", typeof(bool));
		}

		public void RemoveDatable()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			((ItemsControl)(DataGrid)gridName).ItemsSource = null;
			((ItemsControl)(DataGrid)gridTotal).ItemsSource = null;
		}

		public void FillDataTable(DataTable dtCurrent, int grdNo, int sumColumn, bool isFilterApplicable)
		{
			//IL_098a: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b5: Expected O, but got Unknown
			//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c7: Expected O, but got Unknown
			this.isFilterApplicable = isFilterApplicable;
			if (isFilterApplicable)
			{
				if (!IsFiltarable)
				{
					DataView dataView = new DataView(dtCurrent.Copy());
					if (!(gridColumnName[0] != ""))
					{
						return;
					}
					DataTable source = dataView.ToTable(distinct: false, gridColumnName.ToArray());
					List<string> TempList = gridColumnName;
					List<string> list = gridColumnType;
					string text = "group by ";
					if (list.Count > 1)
					{
						for (int i = 0; i < list.Count && list[i].ToUpper() == "STRING"; i++)
						{
							text = text + "'" + TempList[i] + "'";
						}
						switch (sumColumn)
						{
						case 1:
						{
							var enumerable5 = from dataRow in source.AsEnumerable()
								group dataRow by new
								{
									ID = dataRow.Field<string>(TempList[0])
								} into grp
								select new
								{
									name1 = grp.Key.ID,
									sum1 = grp.Sum((DataRow s1) => Convert.ToDouble(s1.ItemArray[2]))
								};
							CreateDataTable();
							BindingDataTable.Rows.Clear();
							{
								foreach (var item in enumerable5)
								{
									DataRow dataRow2 = BindingDataTable.NewRow();
									dataRow2[1] = item.name1;
									dataRow2[2] = item.sum1;
									dataRow2[3] = 0;
									BindingDataTable.Rows.Add(dataRow2);
								}
								break;
							}
						}
						case 2:
						{
							var enumerable4 = from dataRow in source.AsEnumerable()
								group dataRow by new
								{
									ID = dataRow.Field<string>(TempList[0])
								} into grp
								select new
								{
									name1 = grp.Key.ID,
									sum1 = grp.Sum((DataRow s1) => Convert.ToDouble(s1.ItemArray[1])),
									sum2 = grp.Sum((DataRow s2) => Convert.ToDouble(s2.ItemArray[2]))
								};
							CreateDataTable();
							BindingDataTable.Rows.Clear();
							{
								foreach (var item2 in enumerable4)
								{
									DataRow dataRow2 = BindingDataTable.NewRow();
									dataRow2[0] = item2.name1;
									dataRow2[1] = Math.Round(item2.sum1, 3);
									dataRow2[2] = item2.sum2;
									dataRow2[3] = 0;
									BindingDataTable.Rows.Add(dataRow2);
								}
								break;
							}
						}
						case 3:
						{
							var enumerable3 = from dataRow in source.AsEnumerable()
								group dataRow by new
								{
									ID = dataRow.Field<string>(TempList[0])
								} into grp
								select new
								{
									name1 = grp.Key.ID,
									sum1 = grp.Sum((DataRow s1) => Convert.ToDouble(s1.ItemArray[1])),
									sum2 = grp.Sum((DataRow s2) => Convert.ToDouble(s2.ItemArray[2])),
									sum3 = grp.Sum((DataRow s2) => Convert.ToDouble(s2.ItemArray[3]))
								};
							CreateDataTable();
							BindingDataTable.Rows.Clear();
							{
								foreach (var item3 in enumerable3)
								{
									DataRow dataRow2 = BindingDataTable.NewRow();
									dataRow2[0] = item3.name1;
									dataRow2[1] = Math.Round(item3.sum1, 3);
									dataRow2[2] = Math.Round(item3.sum2, 3);
									dataRow2[3] = Math.Round(item3.sum3, 3);
									dataRow2[4] = 0;
									BindingDataTable.Rows.Add(dataRow2);
								}
								break;
							}
						}
						case 4:
						{
							var enumerable2 = from dataRow in source.AsEnumerable()
								group dataRow by new
								{
									ID = dataRow.Field<string>(TempList[0])
								} into grp
								select new
								{
									name1 = grp.Key.ID,
									sum1 = grp.Sum((DataRow s1) => Convert.ToDouble(s1.ItemArray[1])),
									sum2 = grp.Sum((DataRow s2) => Convert.ToDouble(s2.ItemArray[2])),
									sum3 = grp.Sum((DataRow s3) => Convert.ToDouble(s3.ItemArray[3])),
									sum4 = grp.Sum((DataRow s4) => Convert.ToDouble(s4.ItemArray[4]))
								};
							CreateDataTable();
							BindingDataTable.Rows.Clear();
							{
								foreach (var item4 in enumerable2)
								{
									DataRow dataRow2 = BindingDataTable.NewRow();
									dataRow2[0] = item4.name1;
									dataRow2[1] = Math.Round(item4.sum1, 3);
									dataRow2[2] = Math.Round(item4.sum2, 3);
									dataRow2[3] = Math.Round(item4.sum3, 3);
									dataRow2[4] = Math.Round(item4.sum4, 3);
									dataRow2[5] = 0;
									BindingDataTable.Rows.Add(dataRow2);
								}
								break;
							}
						}
						case 7:
						{
							var enumerable6 = from dataRow in source.AsEnumerable()
								group dataRow by new
								{
									ID = dataRow.Field<string>(TempList[0])
								} into grp
								select new
								{
									name1 = grp.Key.ID,
									sum1 = grp.Sum((DataRow s1) => Convert.ToDouble(s1.ItemArray[1])),
									sum2 = grp.Sum((DataRow s2) => Convert.ToDouble(s2.ItemArray[2])),
									sum3 = grp.Sum((DataRow s3) => Convert.ToDouble(s3.ItemArray[3])),
									sum4 = grp.Sum((DataRow s4) => Convert.ToDouble(s4.ItemArray[4])),
									sum5 = grp.Sum((DataRow s5) => Convert.ToDouble(s5.ItemArray[5])),
									sum6 = grp.Sum((DataRow s6) => Convert.ToDouble(s6.ItemArray[6])),
									sum7 = grp.Sum((DataRow s7) => Convert.ToDouble(s7.ItemArray[7]))
								};
							CreateDataTable();
							BindingDataTable.Rows.Clear();
							{
								foreach (var item5 in enumerable6)
								{
									DataRow dataRow2 = BindingDataTable.NewRow();
									dataRow2[0] = item5.name1;
									dataRow2[1] = Math.Round(item5.sum1, 3);
									dataRow2[2] = Math.Round(item5.sum2, 3);
									dataRow2[3] = Math.Round(item5.sum3, 3);
									dataRow2[4] = Math.Round(item5.sum4, 3);
									dataRow2[5] = Math.Round(item5.sum5, 3);
									dataRow2[6] = Math.Round(item5.sum6, 3);
									dataRow2[7] = Math.Round(item5.sum7, 3);
									dataRow2[8] = 0;
									BindingDataTable.Rows.Add(dataRow2);
								}
								break;
							}
						}
						case 8:
						{
							var enumerable = from dataRow in source.AsEnumerable()
								group dataRow by new
								{
									ID = dataRow.Field<string>(TempList[0])
								} into grp
								select new
								{
									name1 = grp.Key.ID,
									sum1 = grp.Sum((DataRow s1) => Convert.ToDouble(s1.ItemArray[1])),
									sum2 = grp.Sum((DataRow s2) => Convert.ToDouble(s2.ItemArray[5])),
									sum3 = grp.Sum((DataRow s3) => Convert.ToDouble(s3.ItemArray[6])),
									sum4 = grp.Sum((DataRow s4) => Convert.ToDouble(s4.ItemArray[7])),
									sum5 = grp.Sum((DataRow s5) => Convert.ToDouble(s5.ItemArray[8])),
									name3 = string.Join(",", grp.Select((DataRow s6) => s6.ItemArray[2]).AsEnumerable().Distinct()),
									name4 = string.Join(",", grp.Select((DataRow s7) => s7.ItemArray[3]).AsEnumerable().Distinct()),
									name5 = string.Join(",", grp.Select((DataRow s8) => s8.ItemArray[4]).AsEnumerable().Distinct())
								};
							CreateDataTable();
							BindingDataTable.Rows.Clear();
							{
								foreach (var item6 in enumerable)
								{
									DataRow dataRow2 = BindingDataTable.NewRow();
									dataRow2[0] = item6.name1;
									dataRow2[1] = Math.Round(item6.sum1, 3);
									dataRow2[2] = item6.name3;
									dataRow2[3] = item6.name4;
									dataRow2[4] = item6.name5;
									dataRow2[5] = Math.Round(item6.sum2, 3);
									dataRow2[6] = Math.Round(item6.sum3, 3);
									dataRow2[7] = Math.Round(item6.sum4, 3);
									dataRow2[8] = Math.Round(item6.sum5, 3);
									dataRow2[9] = 0;
									BindingDataTable.Rows.Add(dataRow2);
								}
								break;
							}
						}
						case 5:
						case 6:
							break;
						}
					}
					else
					{
						BindingDataTable = dataView.ToTable(distinct: false, gridColumnName.ToArray());
					}
				}
				else
				{
					for (int i = 0; i < filterIndex.Count; i++)
					{
						BindingDataTable.Rows[filterIndex[i]]["IsSelected"] = true;
					}
					IsFiltarable = false;
				}
			}
			else if (((FrameworkElement)(Control)GetSetGridName).Name == "CounterSaleDetails")
			{
				SqlConnection val = new SqlConnection(connectionString);
				string text2 = "SELECT     salestype as SalesType, FL_bottles,  FL_amount, CS_bottles,  CS_amount,NAI_Qnty,NAI_amount, Total   FROM  TempCounterSale";
				SqlDataAdapter val2 = new SqlDataAdapter(text2, val);
				DataSet dataSet = new DataSet();
				((DataAdapter)(object)val2).Fill(dataSet);
				dt = dataSet.Tables[0];
			}
		}

		public string GetType(string typeName)
		{
			try
			{
				string text = null;
				return typeName.ToLower() switch
				{
					"string" => typeof(string).ToString(), 
					"float" => typeof(float).ToString(), 
					"int" => typeof(int).ToString(), 
					"bool" => typeof(bool).ToString(), 
					_ => throw new NullReferenceException(), 
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int FindItemIndex(List<object> FindItemDetails)
		{
			bool flag = false;
			for (int i = 0; i < BindingDataTable.Rows.Count; i++)
			{
				for (int j = 0; j < BindingDataTable.Columns.Count; j++)
				{
					if (BindingDataTable.Rows[i][j].ToString() == FindItemDetails[j].ToString())
					{
						flag = true;
						continue;
					}
					flag = false;
					break;
				}
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}
	}
	internal class ClsQueryEditor
	{
		public static string connectionString;

		public string queryString;

		private string query;

		public List<int> sumcolumn = new List<int>();

		private string ColumnWidth;

		public string Title { get; set; }

		public string sp_Name { get; set; }

		public List<string> sp_Arguments { get; set; }

		public int No_of_Filters { get; set; }

		public List<string> Filter_Name { get; set; }

		public List<string> Filter_DataType { get; set; }

		public List<ClsFilterShowData> FilterColumnDetails { get; set; }

		public List<bool> CheckboxVisible { get; set; }

		public void FilterData(DataView dv, DataTable dt, int count, List<bool> isFilterApplicable, string ColumnWidth = null)
		{
			queryString = "";
			query = "";
			this.ColumnWidth = ColumnWidth;
			for (int i = 0; i < No_of_Filters; i++)
			{
				if (FilterColumnDetails[i].QueryString != null)
				{
					query = query + FilterColumnDetails[i].QueryString + " and ";
					queryString = query.Substring(0, query.Length - 5);
				}
			}
			dv.RowFilter = queryString;
			for (int i = 0; i <= No_of_Filters - 1; i++)
			{
				FilterColumnDetails[i].RemoveDatable();
				FilterColumnDetails[i].FillDataTable(dv.ToTable(), i, sumcolumn[i], isFilterApplicable[i]);
				FilterColumnDetails[i].BindDataTable(i, CheckboxVisible[i]);
				FilterColumnDetails[i].AdjustColumnWidth();
			}
		}
	}
}
namespace ABMQueryScreen
{
	public class MainWindow : Window, IComponentConnector
	{
		private ClsQueryEditor QE;

		private ClsFilterShowData obShowData;

		private DataTable dt;

		private DataView dv;

		private DataGrid dataGridTemp;

		private string connectionString;

		private int showCounter = 0;

		private string querryString;

		private string strGrdName = "";

		private List<string> Sp_Name;

		private string billNo;

		private List<string> spArgs = new List<string>();

		private int No_of_Filters;

		private List<string> FilterName = new List<string>();

		private List<string> Filter_DataType = new List<string>();

		private List<string> columnsName = new List<string>();

		private List<string> Filter_Column_DataType = new List<string>();

		private List<string> filterString = new List<string>();

		private List<int> filterIndex = new List<int>();

		private List<string> gridColumnName = new List<string>();

		private List<string> gridColumnType = new List<string>();

		private List<string> gridStyleName = new List<string>();

		private List<string> gridStyle = new List<string>();

		private List<double> top = new List<double>();

		private List<double> left = new List<double>();

		private List<double> height = new List<double>();

		private List<double> width = new List<double>();

		private List<string> columnWidth = new List<string>();

		private List<bool> isFilterApplicable = new List<bool>();

		private List<int> billtype = new List<int>();

		private List<string> Brand = new List<string>();

		private List<string> Manufacturer = new List<string>();

		private bool isNai;

		private object Objectlike;

		private DateTime YearStartDate;

		private DateTime YearEndDate;

		private string ListDetails = "";

		private static int count_Check;

		private bool isChecked = true;

		private bool IsChackedChangeFire = true;

		private double gridWidth = 0.0;

		private bool FilterButtonVisible;

		private List<bool> CheckboxVisible = new List<bool>();

		private List<bool> HaveMouseDoubleClick = new List<bool>();

		private List<bool> HaveOtherEvent = new List<bool>();

		private DataGrid dgv_BillSummary;

		private DataGrid dgv_BillDetail;

		private DataGrid dgv_CounterSale;

		private DataGrid gdv_billtotal;

		private string gridname;

		private int row;

		private List<int> columncount = new List<int>();

		private ReportDocument rpt;

		internal DatePicker dtp_From;

		internal DatePicker dtp_To;

		internal Button btn_Show;

		internal Button btn_Refresh;

		internal Grid grd_Container;

		internal Button btn_Load;

		internal RadioButton rdAI;

		internal RadioButton rdNAI;

		private bool _contentLoaded;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
		}

		public void Formload(string connectionString, string title, List<string> sp_Name, List<string> sp_Arguments, int TotalNoOfFilter, List<string> filterName, List<string> Filter_DataType, List<string> filterColumnName, List<string> Filter_Column_DataType, DateTime YearStrDt, DateTime YearEndDt, List<double> Top, List<double> Left, List<double> Height, List<double> Width, List<string> ColumnWidth, List<bool> HaveMouseDoubleClick, List<bool> HaveOtherEvent, bool FilterButtonVisible, List<bool> CheckboxVisible, List<bool> isFilterApplicable, bool isnai, object edObject = null)
		{
			Objectlike = edObject;
			((Window)this).Topmost = true;
			YearStartDate = YearStrDt;
			YearEndDate = YearEndDt;
			dtp_From.DisplayDateStart = YearStartDate;
			dtp_From.DisplayDateEnd = YearEndDate;
			dtp_To.DisplayDateStart = YearStartDate;
			dtp_To.DisplayDateEnd = YearEndDate;
			this.connectionString = connectionString;
			((Window)this).Title = title;
			Sp_Name = sp_Name;
			spArgs = sp_Arguments;
			No_of_Filters = TotalNoOfFilter;
			FilterName = filterName;
			this.Filter_DataType = Filter_DataType;
			columnsName = filterColumnName;
			this.Filter_Column_DataType = Filter_Column_DataType;
			this.HaveMouseDoubleClick = HaveMouseDoubleClick;
			this.HaveOtherEvent = HaveOtherEvent;
			top = Top;
			left = Left;
			height = Height;
			width = Width;
			columnWidth = ColumnWidth;
			this.FilterButtonVisible = FilterButtonVisible;
			this.CheckboxVisible = CheckboxVisible;
			this.isFilterApplicable = isFilterApplicable;
			isNai = isnai;
			if (((Window)this).Title == "Yearly Sale" || ((Window)this).Title == "Yearly CS Details")
			{
				((UIElement)rdAI).Visibility = (Visibility)1;
				((UIElement)rdNAI).Visibility = (Visibility)1;
			}
			else
			{
				((UIElement)rdAI).Visibility = (Visibility)0;
				((UIElement)rdNAI).Visibility = (Visibility)0;
			}
		}

		public void TableLoad(string sp_Name, List<string> sp_Arguments, int TotalNoOfFilter, List<string> filterName, List<string> filterDataType, List<string> filterColumnName, List<string> filterColumnDataType, List<double> Top, List<double> Left, List<double> Height, List<double> Width, List<bool> HaveMouseDoubleClick, List<bool> HaveOtherEvent)
		{
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Expected O, but got Unknown
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Expected O, but got Unknown
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Expected O, but got Unknown
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_070a: Expected O, but got Unknown
			//IL_0756: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d6: Expected O, but got Unknown
			//IL_081f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0850: Unknown result type (might be due to invalid IL or missing references)
			//IL_0894: Unknown result type (might be due to invalid IL or missing references)
			//IL_089e: Expected O, but got Unknown
			//IL_089f: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a6: Expected O, but got Unknown
			//IL_08de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Expected O, but got Unknown
			//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Expected O, but got Unknown
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Expected O, but got Unknown
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Expected O, but got Unknown
			//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Expected O, but got Unknown
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			((Panel)grd_Container).Children.Clear();
			if (!FilterButtonVisible)
			{
				((UIElement)btn_Show).Visibility = (Visibility)1;
			}
			else
			{
				((UIElement)btn_Show).Visibility = (Visibility)0;
			}
			QE = new ClsQueryEditor();
			QE.Title = ((Window)this).Title;
			QE.sp_Name = sp_Name;
			QE.sp_Arguments = sp_Arguments;
			QE.No_of_Filters = TotalNoOfFilter;
			QE.Filter_Name = filterName;
			QE.Filter_DataType = filterDataType;
			QE.FilterColumnDetails = new List<ClsFilterShowData>();
			SqlConnection val = new SqlConnection(connectionString);
			string text = "'";
			for (int i = 0; i < sp_Arguments.Count; i++)
			{
				text = text + sp_Arguments[i] + "','";
			}
			DataSet dataSet = new DataSet();
			text = "Exec " + sp_Name + text.Substring(0, text.Length - 2);
			SqlDataAdapter val2 = new SqlDataAdapter(text, val);
			((DataAdapter)(object)val2).Fill(dataSet);
			dt = dataSet.Tables[0];
			dv = new DataView(dt);
			if (dt.Rows.Count > 0)
			{
				bool flag = false;
				if (left.Sum((double x) => x) > 0.0)
				{
					flag = true;
				}
				for (int i = 0; i < TotalNoOfFilter; i++)
				{
					Grid val3 = new Grid();
					((FrameworkElement)val3).Name = "grid_" + filterName[i].Replace(" ", "");
					((FrameworkElement)val3).HorizontalAlignment = (HorizontalAlignment)0;
					((FrameworkElement)val3).VerticalAlignment = (VerticalAlignment)0;
					if (flag)
					{
						double num = 0.0;
						double num2 = 0.0;
						double num3 = 0.0;
						double num4 = 0.0;
						try
						{
							num = double.Parse(Width[i].ToString());
						}
						catch (Exception)
						{
						}
						try
						{
							num2 = double.Parse(Height[i].ToString());
						}
						catch (Exception)
						{
						}
						try
						{
							num3 = double.Parse(Left[i].ToString());
						}
						catch (Exception)
						{
						}
						try
						{
							num4 = double.Parse(Top[i].ToString());
						}
						catch (Exception)
						{
						}
						((FrameworkElement)val3).Width = Width[i];
						if (Height[i] > 0.0)
						{
							((FrameworkElement)val3).Height = Height[i];
						}
						else
						{
							((FrameworkElement)val3).Height = double.NaN;
						}
						((FrameworkElement)val3).Margin = new Thickness(num3, num4, 5.0, 0.0);
					}
					else
					{
						((FrameworkElement)val3).Width = SystemParameters.PrimaryScreenWidth / (double)TotalNoOfFilter;
						((FrameworkElement)val3).Height = double.NaN;
						((FrameworkElement)val3).Margin = new Thickness(SystemParameters.PrimaryScreenWidth / (double)TotalNoOfFilter, 0.0, 5.0, 0.0);
					}
					DataGrid val4 = new DataGrid();
					((FrameworkElement)val4).Name = "Total_" + filterName[i].Replace(" ", "");
					((FrameworkElement)val4).HorizontalAlignment = (HorizontalAlignment)2;
					((FrameworkElement)val4).VerticalAlignment = (VerticalAlignment)2;
					((FrameworkElement)val4).Margin = new Thickness(0.0, 0.0, 15.0, 0.0);
					((FrameworkElement)val4).Width = double.NaN;
					((FrameworkElement)val4).Height = 20.0;
					val4.HeadersVisibility = (DataGridHeadersVisibility)0;
					val4.CanUserAddRows = false;
					val4.AutoGenerateColumns = false;
					val4.CanUserResizeRows = false;
					val4.CanUserResizeColumns = false;
					val4.CanUserDeleteRows = false;
					DataGrid val5 = new DataGrid();
					((FrameworkElement)val5).Name = filterName[i].Replace(" ", "");
					((FrameworkElement)val5).HorizontalAlignment = (HorizontalAlignment)2;
					((FrameworkElement)val5).VerticalAlignment = (VerticalAlignment)3;
					((FrameworkElement)val5).Margin = new Thickness(0.0, 30.0, 0.0, 20.0);
					((FrameworkElement)val5).Width = double.NaN;
					((FrameworkElement)val5).Height = double.NaN;
					val5.CanUserAddRows = false;
					val5.AutoGenerateColumns = false;
					val5.CanUserResizeRows = false;
					val5.CanUserDeleteRows = false;
					val5.CanUserResizeColumns = false;
					val5.RowDetailsVisibilityMode = (DataGridRowDetailsVisibilityMode)1;
					val5.HorizontalGridLinesBrush = (Brush)(object)Brushes.LightGray;
					val5.VerticalGridLinesBrush = (Brush)(object)Brushes.WhiteSmoke;
					val5.VerticalScrollBarVisibility = (ScrollBarVisibility)3;
					((Control)val5).FontSize = 13.0;
					if (HaveMouseDoubleClick[i])
					{
						((UIElement)val5).MouseLeftButtonDown += new MouseButtonEventHandler(MouseDoubleClick);
					}
					if (HaveOtherEvent[i])
					{
						((UIElement)val5).AddHandler(ToggleButton.CheckedEvent, (Delegate)new RoutedEventHandler(Checked_Changed));
						((UIElement)val5).AddHandler(ToggleButton.UncheckedEvent, (Delegate)new RoutedEventHandler(UnChecked_Changed));
						((UIElement)val5).AddHandler(UIElement.MouseUpEvent, (Delegate)new RoutedEventHandler(select_change));
						((UIElement)val5).AddHandler(UIElement.KeyDownEvent, (Delegate)new RoutedEventHandler(select_change));
						((UIElement)val5).AddHandler(UIElement.KeyUpEvent, (Delegate)new RoutedEventHandler(select_change));
					}
					obShowData = new ClsFilterShowData(filterColumnName[i].Split(new char[1] { ',' }).ToList(), filterColumnDataType[i].Split(new char[1] { ',' }).ToList());
					obShowData.GetSetGridName = val5;
					obShowData.GetSetGridTotal = val4;
					columncount.Add(obShowData.GetSetGridColumnName.Count - 1);
					QE.sumcolumn.Add(obShowData.GetSetGridColumnName.Count - 1);
					obShowData.IsFiltarable = false;
					obShowData.columnWidth = columnWidth[i];
					obShowData.connectionString = connectionString;
					QE.FilterColumnDetails.Add(obShowData);
					QE.CheckboxVisible = CheckboxVisible;
					Label val6 = new Label();
					((FrameworkElement)val6).Name = "lbl_" + filterName[i].Replace(" ", "");
					((ContentControl)val6).Content = filterName[i];
					((Control)val6).FontSize = 18.0;
					((Control)val6).FontWeight = FontWeights.Bold;
					((FrameworkElement)val6).HorizontalAlignment = (HorizontalAlignment)3;
					((FrameworkElement)val6).VerticalAlignment = (VerticalAlignment)0;
					((Control)val6).HorizontalContentAlignment = (HorizontalAlignment)0;
					((FrameworkElement)val6).Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
					((FrameworkElement)val6).Height = 33.0;
					((FrameworkElement)val6).Width = double.NaN;
					Button val7 = new Button();
					((FrameworkElement)val7).Name = "btn_" + filterName[i].Replace(" ", "");
					((ContentControl)val7).Content = "Print";
					((Control)val7).FontSize = 18.0;
					((Control)val7).FontWeight = FontWeights.Bold;
					((FrameworkElement)val7).HorizontalAlignment = (HorizontalAlignment)2;
					((FrameworkElement)val7).VerticalAlignment = (VerticalAlignment)0;
					((Control)val7).HorizontalContentAlignment = (HorizontalAlignment)1;
					((FrameworkElement)val7).Margin = new Thickness(1.0);
					((FrameworkElement)val7).Height = 30.0;
					((FrameworkElement)val7).Width = 50.0;
					((UIElement)val7).Visibility = (Visibility)1;
					((UIElement)val7).AddHandler(ButtonBase.ClickEvent, (Delegate)new RoutedEventHandler(btn_Print_Click));
					Border val8 = new Border();
					((FrameworkElement)val8).HorizontalAlignment = (HorizontalAlignment)3;
					((FrameworkElement)val8).VerticalAlignment = (VerticalAlignment)3;
					val8.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
					val8.BorderBrush = (Brush)(object)Brushes.WhiteSmoke;
					((Panel)val3).Children.Add((UIElement)(object)val6);
					((Panel)val3).Children.Add((UIElement)(object)val7);
					((Panel)val3).Children.Add((UIElement)(object)val5);
					((Panel)val3).Children.Add((UIElement)(object)val4);
					((Panel)val3).Children.Add((UIElement)(object)val8);
					((Panel)grd_Container).Children.Add((UIElement)(object)val3);
				}
				columncount.Clear();
				QE.FilterData(dv, dt, showCounter, isFilterApplicable);
			}
			else
			{
				MessageBox.Show("No Records Found");
			}
		}

		private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_0874: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Expected O, but got Unknown
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Expected O, but got Unknown
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Expected O, but got Unknown
			//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_090c: Unknown result type (might be due to invalid IL or missing references)
			//IL_092f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0939: Expected O, but got Unknown
			//IL_093b: Unknown result type (might be due to invalid IL or missing references)
			//IL_094b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0951: Expected O, but got Unknown
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
			//IL_077b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0782: Expected O, but got Unknown
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Expected O, but got Unknown
			gridname = ((FrameworkElement)(DataGrid)sender).Name;
			if (((FrameworkElement)(DataGrid)sender).Name.ToLower() == "supplier_wisedetails")
			{
				((UIElement)(Grid)((Panel)grd_Container).Children[3]).Visibility = (Visibility)0;
				dgv_BillSummary = (DataGrid)QE.FilterColumnDetails[3].GetSetGridName;
				row = ((Selector)(DataGrid)sender).SelectedIndex;
				DataGrid val = (DataGrid)sender;
				if (row > -1)
				{
					string text = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
					string text2 = "";
					string text3 = "";
					string text4 = "";
					if (Brand.Count > 0)
					{
						for (int i = 0; i < Brand.Count; i++)
						{
							text3 = text3 + "'" + Brand[i].ToString() + "' ,";
							text4 = text3.Substring(0, text3.Length - 1);
						}
						text2 = " and categoryname in (" + text4 + ")";
					}
					else if (Manufacturer.Count > 0)
					{
						for (int i = 0; i < Manufacturer.Count; i++)
						{
							text3 = text3 + "'" + Manufacturer[i].ToString() + "' ,";
							text4 = text3.Substring(0, text3.Length - 1);
						}
						text2 = " and ManufacturerName in (" + text4 + ")";
					}
					SqlConnection val2 = new SqlConnection(connectionString);
					string text5 = "";
					if (((ToggleButton)rdAI).IsChecked == true)
					{
						text5 = "select distinct docno as Billno,convert(varchar,purchasemain.trndate,103) as Date, totnetamt as Amount   from purchasemain   join purchasedetail on purchasemain.companycode = purchasedetail.companycode and purchasemain.yearcode = purchasedetail.yearcode and purchasemain.trnno = purchasedetail.trnno join MasterAccountsLedger on purchasemain.companycode = MasterAccountsLedger.companyCode and purchasemain.suppliercode = MasterAccountsLedger.ledgerCode   join itemmst on purchasedetail.companycode = itemmst.companycode and purchasedetail.itemcode = itemmst.itemcode   join categorymst on itemmst.companycode = categorymst.companycode and itemmst.categorycode = categorymst.categorycode   join ManufacturerMaster on categorymst.companycode = ManufacturerMaster.Companycode and categorymst.ManufacturerCode =  ManufacturerMaster.ManufacturerCode   where purchasemain.companycode = '" + spArgs[0].ToString() + "' and purchasemain.yearCode = '" + spArgs[1].ToString() + "' and docdate >= '" + dtp_From.SelectedDate.Value.ToString("yyyy-MM-dd") + "' and docdate <= '" + dtp_To.SelectedDate.Value.ToString("yyyy-MM-dd") + "' and  MasterAccountsLedger.ledgerName  = '" + text + "'" + text2;
					}
					else if (((ToggleButton)rdNAI).IsChecked == true)
					{
						text5 = "select distinct docno as Billno,convert(varchar,purchasemain.trndate,103) as Date, totnetamt as Amount   from purchasemain      join purchasedetail on purchasemain.companycode = purchasedetail.companycode and purchasemain.yearcode = purchasedetail.yearcode and purchasemain.trnno = purchasedetail.trnno    join MasterAccountsLedger on purchasemain.companycode = MasterAccountsLedger.companyCode and purchasemain.suppliercode = MasterAccountsLedger.ledgerCode      join naiItemMaster  on purchasedetail.companycode = naiItemMaster.companycode and purchasedetail.itemcode = naiItemMaster.itemcode   join naiItemSubGroup on naiItemMaster.companycode = naiItemSubGroup.companycode and naiItemMaster.subGroupCode = naiItemSubGroup.subGroupCode  where purchasemain.companycode = '" + spArgs[0].ToString() + "' and purchasemain.yearCode = '" + spArgs[1].ToString() + "' and docdate >= '" + dtp_From.SelectedDate.Value.ToString("yyyy-MM-dd") + "' and docdate <= '" + dtp_To.SelectedDate.Value.ToString("yyyy-MM-dd") + "' and billType = 'NAI' and  MasterAccountsLedger.ledgerName  = '" + text + "'" + text2;
					}
					SqlDataAdapter val3 = new SqlDataAdapter(text5, val2);
					DataSet dataSet = new DataSet();
					((DataAdapter)(object)val3).Fill(dataSet);
					dt = dataSet.Tables[0];
					for (int j = 0; j < dataSet.Tables[0].Columns.Count; j++)
					{
						gridColumnName.Add(dataSet.Tables[0].Columns[j].ColumnName);
						gridColumnType.Add(dataSet.Tables[0].Columns[j].GetType().ToString());
					}
					BindDataTable(dgv_BillSummary);
					LoadTotalAmount();
					QE.FilterColumnDetails[3].AdjustColumnWidth();
					gridColumnName.Clear();
					gridColumnType.Clear();
					Brand.Clear();
					Manufacturer.Clear();
				}
			}
			else if (((FrameworkElement)(DataGrid)sender).Name.ToLower() == "billsummary")
			{
				((UIElement)(Grid)((Panel)grd_Container).Children[4]).Visibility = (Visibility)0;
				dgv_BillDetail = (DataGrid)QE.FilterColumnDetails[4].GetSetGridName;
				row = ((Selector)(DataGrid)sender).SelectedIndex;
				DataGrid val = (DataGrid)sender;
				string text6 = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
				SqlConnection val2 = new SqlConnection(connectionString);
				string text5 = "";
				text5 = ((((ToggleButton)rdAI).IsChecked != true) ? ("SELECT   purchasemain.docno AS Billno,naiItemMaster.itemName as Item, batchno as BatchNo,sum(purchasedetail.itembox) as [Case] , sum(purchasedetail.itemloose) AS Bottle, SUM(purchasedetail.itemfreeqnty) as FreeQnty, purchasedetail.itemrate AS Rate, sum(purchasedetail.itemdiscount) as Discount, sum(purchasedetail.itemamount) AS Amount  from purchasemain   inner join purchasedetail on purchasemain.companycode = purchasedetail.companycode and purchasemain.yearcode = purchasedetail.yearcode and purchasemain.trnno = purchasedetail.trnno   INNER JOIN naiItemMaster ON purchasedetail.itemcode = naiItemMaster.itemcode AND purchasedetail.companycode = naiItemMaster.companycode where purchasemain.companycode='" + spArgs[0].ToString() + "' and purchasemain.yearcode='" + spArgs[1].ToString() + "' and purchasemain.trndate >= '" + dtp_From.SelectedDate.Value.ToString("yyyy-MM-dd") + "' and purchasemain.trndate <= '" + dtp_To.SelectedDate.Value.ToString("yyyy-MM-dd") + "'  and purchasemain.docno = '" + text6 + "' group by purchasemain.trndate, purchasemain.docno,naiItemMaster.itemname, batchno,purchasedetail.itemrate") : ("SELECT   purchasemain.docno AS Billno,itemmst.itemname as Item, batchno as BatchNo,sum(purchasedetail.itembox) as [Case] , sum(purchasedetail.itemloose) AS Bottle, SUM(purchasedetail.itemfreeqnty) as FreeQnty, purchasedetail.itemrate AS Rate, sum(purchasedetail.itemdiscount) as Discount, sum(purchasedetail.itemamount) AS Amount  from purchasemain inner join purchasedetail on purchasemain.companycode = purchasedetail.companycode and purchasemain.yearcode = purchasedetail.yearcode and purchasemain.trnno = purchasedetail.trnno INNER JOIN itemmst ON purchasedetail.itemcode = itemmst.itemcode AND purchasedetail.companycode = itemmst.companycode where purchasemain.companycode='" + spArgs[0].ToString() + "' and purchasemain.yearcode='" + spArgs[1].ToString() + "' and purchasemain.trndate >= '" + dtp_From.SelectedDate.Value.ToString("yyyy-MM-dd") + "' and purchasemain.trndate <= '" + dtp_To.SelectedDate.Value.ToString("yyyy-MM-dd") + "'  and purchasemain.docno = '" + text6 + "' group by purchasemain.trndate, purchasemain.docno,itemmst.itemname, batchno,purchasedetail.itemrate"));
				SqlDataAdapter val3 = new SqlDataAdapter(text5, val2);
				DataSet dataSet2 = new DataSet();
				((DataAdapter)(object)val3).Fill(dataSet2);
				dt = dataSet2.Tables[0];
				for (int j = 0; j < dataSet2.Tables[0].Columns.Count; j++)
				{
					gridColumnName.Add(dataSet2.Tables[0].Columns[j].ColumnName);
					gridColumnType.Add(dataSet2.Tables[0].Columns[j].GetType().ToString());
				}
				BindDataTable(dgv_BillDetail);
				QE.FilterColumnDetails[4].AdjustColumnWidth();
				gridColumnName.Clear();
				gridColumnType.Clear();
			}
			else if (((FrameworkElement)(DataGrid)sender).Name.ToLower() == "billwisetotal")
			{
				((UIElement)(Button)((Panel)(Grid)((Panel)grd_Container).Children[4]).Children[1]).Visibility = (Visibility)0;
				string dynamicgridname = ((FrameworkElement)(DataGrid)sender).Name.ToLower();
				((UIElement)(Grid)((Panel)grd_Container).Children[4]).Visibility = (Visibility)0;
				((ItemsControl)(DataGrid)QE.FilterColumnDetails[4].GetSetGridName).ItemsSource = null;
				dgv_CounterSale = (DataGrid)QE.FilterColumnDetails[4].GetSetGridName;
				row = ((Selector)(DataGrid)sender).SelectedIndex;
				DataGrid val = (DataGrid)sender;
				string text7 = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
				billNo = text7.Substring(4);
				dv.RowFilter = "BillNo = '" + text7 + "'";
				dt = dv.ToTable().Copy();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					billtype.Add(Convert.ToInt32(dt.Rows[i]["billtype"]));
				}
				BindDataTable(dgv_CounterSale, dynamicgridname);
				LoadTotalAmount(dynamicgridname);
				QE.FilterColumnDetails[4].AdjustColumnWidth();
				((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[4]).Children[2]).Columns[1].Header = "Qnty";
			}
			strGrdName = "";
		}

		public void BindDataTable(DataGrid dgv, string dynamicgridname = null)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			dgv.Columns.Clear();
			if (dynamicgridname == "billwisetotal")
			{
				gridColumnName = QE.FilterColumnDetails[4].GetSetGridColumnName;
				gridColumnType = QE.FilterColumnDetails[4].GetSetGirdColumnType;
			}
			for (int i = 0; i < gridColumnName.Count; i++)
			{
				DataGridTextColumn val = new DataGridTextColumn();
				((DataGridColumn)val).Header = gridColumnName[i].ToString();
				((DataGridBoundColumn)val).Binding = (BindingBase)new Binding(gridColumnName[i].ToString());
				dgv.Columns.Add((DataGridColumn)(object)val);
				((DataGridColumn)val).IsReadOnly = true;
			}
			((ItemsControl)dgv).ItemsSource = dt.DefaultView;
			((CollectionView)((ItemsControl)dgv).Items).Refresh();
		}

		public void LoadTotalAmount(string dynamicgridname = null)
		{
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Expected O, but got Unknown
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Expected O, but got Unknown
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Expected O, but got Unknown
			double num = 0.0;
			double num2 = 0.0;
			DataGrid val;
			DataTable dataTable;
			DataRow dataRow;
			if (dynamicgridname == "billwisetotal")
			{
				val = (DataGrid)QE.FilterColumnDetails[4].GetSetGridTotal;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					num += Convert.ToDouble(dt.Rows[i][5].ToString());
					try
					{
						num2 += Convert.ToDouble(dt.Rows[i][7].ToString());
					}
					catch
					{
					}
				}
				dataTable = new DataTable();
				dataTable.TableName = gridname;
				dataTable.Columns.Add("Total", typeof(string));
				for (int i = 0; i < gridColumnName.Count - 1; i++)
				{
					dataTable.Columns.Add(gridColumnName[i + 1].ToString(), gridColumnType[i + 1].GetType());
				}
				dataRow = dataTable.NewRow();
				dataRow["Total"] = "Total";
				dataRow[gridColumnName[1]] = Math.Round(num, 2);
				try
				{
					dataRow[gridColumnName[2]] = Math.Round(num2, 2);
				}
				catch
				{
				}
				dataTable.Rows.Add(dataRow);
				val.Columns.Clear();
				((FrameworkElement)val).DataContext = dataTable;
				for (int j = 0; j < dataTable.Columns.Count; j++)
				{
					DataGridTextColumn val2 = new DataGridTextColumn();
					((DataGridColumn)val2).Header = dataTable.Columns[j].ToString();
					((DataGridBoundColumn)val2).Binding = (BindingBase)new Binding(dataTable.Columns[j].ToString());
					val.Columns.Add((DataGridColumn)(object)val2);
					((DataGridColumn)val2).IsReadOnly = true;
				}
				((ItemsControl)val).ItemsSource = dataTable.DefaultView;
				((CollectionView)((ItemsControl)val).Items).Refresh();
				return;
			}
			val = (DataGrid)QE.FilterColumnDetails[3].GetSetGridTotal;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				string text = dt.Rows[i][1].ToString();
				num += Convert.ToDouble(dt.Rows[i][2].ToString());
				try
				{
					num2 += Convert.ToDouble(dt.Rows[i][3].ToString());
				}
				catch
				{
				}
			}
			dataTable = new DataTable();
			dataTable.TableName = gridname;
			dataTable.Columns.Add("Total", typeof(string));
			for (int i = 0; i < gridColumnName.Count - 1; i++)
			{
				dataTable.Columns.Add(gridColumnName[i + 1].ToString(), gridColumnType[i + 1].GetType());
			}
			dataRow = dataTable.NewRow();
			dataRow["Total"] = "Total";
			dataRow[gridColumnName[2]] = Math.Round(num, 2);
			try
			{
				dataRow[gridColumnName[3]] = Math.Round(num2, 2);
			}
			catch
			{
			}
			dataTable.Rows.Add(dataRow);
			val.Columns.Clear();
			((FrameworkElement)val).DataContext = dataTable;
			for (int j = 0; j < dataTable.Columns.Count; j++)
			{
				DataGridTextColumn val2 = new DataGridTextColumn();
				((DataGridColumn)val2).Header = dataTable.Columns[j].ToString();
				((DataGridBoundColumn)val2).Binding = (BindingBase)new Binding(dataTable.Columns[j].ToString());
				val.Columns.Add((DataGridColumn)(object)val2);
				((DataGridColumn)val2).IsReadOnly = true;
			}
			((ItemsControl)val).ItemsSource = dataTable.DefaultView;
			((CollectionView)((ItemsControl)val).Items).Refresh();
		}

		private void select_change(object sender, EventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			DataGrid val = (DataGrid)sender;
			if (!isChecked && strGrdName != ((FrameworkElement)val).Name.ToString())
			{
				strGrdName = ((FrameworkElement)val).Name.ToString();
				filterString.Clear();
				filterIndex.Clear();
				isChecked = true;
			}
			if (strGrdName != "" && strGrdName != ((FrameworkElement)val).Name.ToString())
			{
				((Selector)val).SelectedItem = null;
			}
		}

		private void UnChecked_Changed(object sender, RoutedEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			if (IsChackedChangeFire)
			{
				DataGrid val = (DataGrid)sender;
				string item = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
				int selectedIndex = ((Selector)val).SelectedIndex;
				filterString.Remove(item);
				filterIndex.Remove(selectedIndex);
				count_Check--;
				if (count_Check == 0)
				{
					strGrdName = "";
				}
			}
		}

		private void Checked_Changed(object sender, RoutedEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			if (!IsChackedChangeFire)
			{
				return;
			}
			IsChackedChangeFire = false;
			DataGrid val = (DataGrid)sender;
			if (!isChecked && strGrdName != ((FrameworkElement)val).Name.ToString())
			{
				strGrdName = ((FrameworkElement)val).Name.ToString();
				isChecked = true;
			}
			object selectedItem = ((Selector)val).SelectedItem;
			if (strGrdName == "")
			{
				strGrdName = ((FrameworkElement)val).Name.ToString();
				string item = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
				int selectedIndex = ((Selector)val).SelectedIndex;
				(val.CurrentItem as DataRowView).Row.ItemArray[val.Columns.Count - 1] = true;
				filterIndex.Add(selectedIndex);
				filterString.Add(item);
				count_Check++;
			}
			else
			{
				if (strGrdName != ((FrameworkElement)val).Name.ToString())
				{
					((Selector)val).SelectedItem = null;
					return;
				}
				int index = QE.Filter_Name.FindIndex((string x) => x.Replace(" ", "") == strGrdName);
				if (val.CurrentItem != null)
				{
					try
					{
						if (bool.Parse((val.CurrentItem as DataRowView).Row.ItemArray[val.Columns.Count - 1].ToString()))
						{
							string item = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
							filterString.Remove(item);
							int selectedIndex = QE.FilterColumnDetails[index].FindItemIndex((val.CurrentItem as DataRowView).Row.ItemArray.ToList());
							QE.FilterColumnDetails[index].BindingDataTable.Rows[selectedIndex]["IsSelected"] = false;
							filterIndex.Remove(selectedIndex);
							count_Check--;
						}
						else
						{
							string item = (val.CurrentItem as DataRowView).Row.ItemArray[0].ToString();
							filterString.Add(item);
							int selectedIndex = QE.FilterColumnDetails[index].FindItemIndex((val.CurrentItem as DataRowView).Row.ItemArray.ToList());
							QE.FilterColumnDetails[index].BindingDataTable.Rows[selectedIndex]["IsSelected"] = true;
							filterIndex.Add(selectedIndex);
							count_Check++;
						}
					}
					catch
					{
					}
				}
			}
			IsChackedChangeFire = true;
			if (((FrameworkElement)(DataGrid)sender).Name.ToLower() == "brand_wisedetails")
			{
				string[] array = new string[filterString.Count];
				filterString.CopyTo(array);
				Brand = array.ToList();
			}
			else if (((FrameworkElement)(DataGrid)sender).Name.ToLower() == "manufacturer_wisedetails")
			{
				string[] array = new string[filterString.Count];
				filterString.CopyTo(array);
				Manufacturer = array.ToList();
			}
		}

		private void btn_Show_Click(object sender, RoutedEventArgs e)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_078b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06da: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_075e: Unknown result type (might be due to invalid IL or missing references)
			if (!isNai)
			{
				((UIElement)rdAI).Visibility = (Visibility)1;
				((UIElement)rdNAI).Visibility = (Visibility)1;
			}
			else
			{
				((UIElement)rdAI).Visibility = (Visibility)0;
				((UIElement)rdNAI).Visibility = (Visibility)0;
			}
			try
			{
				((UIElement)(Button)((Panel)(Grid)((Panel)grd_Container).Children[4]).Children[1]).Visibility = (Visibility)1;
			}
			catch
			{
			}
			count_Check = 0;
			if (strGrdName != "")
			{
				DataTable dataTable = new DataTable(strGrdName);
				for (int i = 0; i < ((Panel)grd_Container).Children.Count; i++)
				{
					string text = ((object)((Panel)(Grid)((Panel)grd_Container).Children[i]).Children[2]).GetType().ToString();
					if (((object)((Panel)(Grid)((Panel)grd_Container).Children[i]).Children[2]).GetType().ToString() == "System.Windows.Controls.DataGrid")
					{
						if (((FrameworkElement)(DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[i]).Children[2]).Name.ToLower() == strGrdName.ToLower())
						{
						}
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[i]).Children[2]).Columns.Clear();
					}
				}
				int index = QE.Filter_Name.FindIndex((string x) => x.Replace(" ", "") == strGrdName);
				string text2 = "";
				for (int i = 0; i < filterString.Count; i++)
				{
					text2 = text2 + "'" + filterString[i].ToString() + "' ,";
					ListDetails = text2.Substring(0, text2.Length - 1);
				}
				showCounter += showCounter;
				QE.FilterColumnDetails[index].QueryString = QE.FilterColumnDetails[index].GetSetGridColumnName[0] + " in (" + ListDetails + ")";
				QE.FilterColumnDetails[index].IsFiltarable = true;
				QE.FilterColumnDetails[index].filterIndex = filterIndex;
				QE.FilterData(dv, dt, showCounter, isFilterApplicable);
				isChecked = false;
			}
			else
			{
				MessageBox.Show("Select fields to filter");
			}
			try
			{
				if (((Panel)grd_Container).Children.Count > 0)
				{
					if (No_of_Filters > 2 && HaveMouseDoubleClick[2])
					{
						((UIElement)(Grid)((Panel)grd_Container).Children[3]).Visibility = (Visibility)1;
					}
					if (No_of_Filters > 3 && HaveMouseDoubleClick[3])
					{
						((UIElement)(Grid)((Panel)grd_Container).Children[4]).Visibility = (Visibility)1;
					}
					if (No_of_Filters > 4 && HaveMouseDoubleClick[4])
					{
						((UIElement)(Grid)((Panel)grd_Container).Children[5]).Visibility = (Visibility)1;
					}
				}
				if (((ToggleButton)rdNAI).IsChecked == true)
				{
					((UIElement)(Grid)((Panel)grd_Container).Children[0]).Visibility = (Visibility)1;
					if (((Window)this).Title == "Sales Query")
					{
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[2].Header = "Qnty";
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[3]).Children[2]).Columns[2].Header = "Qnty";
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[2].Header = "Qnty";
					}
					else if (((Window)this).Title == "Stock Query")
					{
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[2].Header = "Closing";
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[3].Visibility = (Visibility)1;
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[2].Header = "Closing";
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[3].Visibility = (Visibility)1;
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[3]).Children[2]).Columns[1].Header = "Pcs";
					}
					else if (((Window)this).Title == "Scheme")
					{
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[3].Header = "Qnty";
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[2].Visibility = (Visibility)1;
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[3].Header = "Qnty";
						((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[2].Visibility = (Visibility)1;
					}
				}
				else
				{
					((UIElement)(Grid)((Panel)grd_Container).Children[0]).Visibility = (Visibility)0;
				}
			}
			catch (Exception)
			{
			}
		}

		private void btn_Refresh_Click(object sender, RoutedEventArgs e)
		{
			showCounter = 0;
			List<int> list = new List<int>();
			List<string> list2 = new List<string>();
		}

		private void btn_Load_Click(object sender, RoutedEventArgs e)
		{
			//IL_078a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0753: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_0619: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0676: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06da: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0716: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			count_Check = 0;
			((Panel)grd_Container).Children.Clear();
			if (!isNai)
			{
				((UIElement)rdAI).Visibility = (Visibility)1;
				((UIElement)rdNAI).Visibility = (Visibility)1;
			}
			else
			{
				((UIElement)rdAI).Visibility = (Visibility)0;
				((UIElement)rdNAI).Visibility = (Visibility)0;
			}
			strGrdName = "";
			if (dtp_From.SelectedDate.HasValue && dtp_To.SelectedDate.HasValue)
			{
				spArgs[2] = dtp_From.SelectedDate.Value.Year + "-" + dtp_From.SelectedDate.Value.Month + "-" + dtp_From.SelectedDate.Value.Day;
				spArgs[3] = dtp_To.SelectedDate.Value.Year + "-" + dtp_To.SelectedDate.Value.Month + "-" + dtp_To.SelectedDate.Value.Day;
				if (((ToggleButton)rdAI).IsChecked == true)
				{
					TableLoad(Sp_Name[0], spArgs, No_of_Filters, FilterName, Filter_DataType, columnsName, Filter_Column_DataType, top, left, height, width, HaveMouseDoubleClick, HaveOtherEvent);
				}
				else if (((ToggleButton)rdNAI).IsChecked == true)
				{
					TableLoad(Sp_Name[1], spArgs, No_of_Filters, FilterName, Filter_DataType, columnsName, Filter_Column_DataType, top, left, height, width, HaveMouseDoubleClick, HaveOtherEvent);
				}
				try
				{
					if (((Panel)grd_Container).Children.Count > 0)
					{
						if (No_of_Filters > 2 && HaveMouseDoubleClick[2])
						{
							((UIElement)(Grid)((Panel)grd_Container).Children[3]).Visibility = (Visibility)1;
						}
						if (No_of_Filters > 3 && HaveMouseDoubleClick[3])
						{
							((UIElement)(Grid)((Panel)grd_Container).Children[4]).Visibility = (Visibility)1;
						}
						if (No_of_Filters > 4 && HaveMouseDoubleClick[4])
						{
							((UIElement)(Grid)((Panel)grd_Container).Children[5]).Visibility = (Visibility)1;
						}
					}
					if (((ToggleButton)rdNAI).IsChecked == true)
					{
						((UIElement)(Grid)((Panel)grd_Container).Children[0]).Visibility = (Visibility)1;
						if (((Window)this).Title == "Sales Query")
						{
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[2].Header = "Qnty";
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[3]).Children[2]).Columns[2].Header = "Qnty";
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[2].Header = "Qnty";
						}
						else if (((Window)this).Title == "Stock Query")
						{
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[2].Header = "Closing";
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[3].Visibility = (Visibility)1;
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[2].Header = "Closing";
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[3].Visibility = (Visibility)1;
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[3]).Children[2]).Columns[1].Header = "Pcs";
						}
						else if (((Window)this).Title == "Scheme")
						{
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[3].Header = "Qnty";
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[1]).Children[2]).Columns[2].Visibility = (Visibility)1;
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[3].Header = "Qnty";
							((DataGrid)((Panel)(Grid)((Panel)grd_Container).Children[2]).Children[2]).Columns[2].Visibility = (Visibility)1;
						}
					}
					else
					{
						((UIElement)(Grid)((Panel)grd_Container).Children[0]).Visibility = (Visibility)0;
					}
				}
				catch (Exception)
				{
				}
				filterString.Clear();
				filterIndex.Clear();
			}
			else
			{
				MessageBox.Show("Please Select Date.");
			}
		}

		private void dtp_From_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			dtp_To.DisplayDateStart = dtp_From.SelectedDate;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			((Window)this).Topmost = false;
		}

		private void btn_Print_Click(object sender, RoutedEventArgs e)
		{
			Type type = Objectlike.GetType();
			MethodInfo method = type.GetMethod("printFromQueryScreen");
			if (((ToggleButton)rdNAI).IsChecked == true)
			{
				method.Invoke(Objectlike, new object[6]
				{
					"Sale",
					dtp_From.SelectedDate,
					dtp_To.SelectedDate,
					billNo,
					billtype[0],
					false
				});
			}
			else
			{
				method.Invoke(Objectlike, new object[6]
				{
					"Sale",
					dtp_From.SelectedDate,
					dtp_To.SelectedDate,
					billNo,
					billtype[0],
					true
				});
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri uri = new Uri("/ABMQueryScreen;component/mainwindow.xaml", UriKind.Relative);
				Application.LoadComponent((object)this, uri);
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			switch (connectionId)
			{
			case 1:
				((FrameworkElement)(MainWindow)target).Loaded += new RoutedEventHandler(Window_Loaded);
				break;
			case 2:
				dtp_From = (DatePicker)target;
				dtp_From.SelectedDateChanged += dtp_From_SelectedDateChanged;
				break;
			case 3:
				dtp_To = (DatePicker)target;
				break;
			case 4:
				btn_Show = (Button)target;
				((ButtonBase)btn_Show).Click += new RoutedEventHandler(btn_Show_Click);
				break;
			case 5:
				btn_Refresh = (Button)target;
				((ButtonBase)btn_Refresh).Click += new RoutedEventHandler(btn_Refresh_Click);
				break;
			case 6:
				grd_Container = (Grid)target;
				break;
			case 7:
				btn_Load = (Button)target;
				((ButtonBase)btn_Load).Click += new RoutedEventHandler(btn_Load_Click);
				break;
			case 8:
				rdAI = (RadioButton)target;
				break;
			case 9:
				rdNAI = (RadioButton)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
namespace ABMQueryScreen.Properties
{
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("ABMQueryScreen.Properties.Resources", typeof(Resources).Assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal Resources()
		{
		}
	}
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)(object)SettingsBase.Synchronized((SettingsBase)(object)new Settings());

		public static Settings Default => defaultInstance;
	}
}
