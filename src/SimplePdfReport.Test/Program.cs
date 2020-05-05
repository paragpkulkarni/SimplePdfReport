using System;
using System.Diagnostics;
using System.IO;
using SimplePdfReport.Reporting;
using SimplePdfReport.Reporting.MigraDoc;

namespace SimplePdfReport.Test
{
	internal class Program
	{
		private static int start = 0;
		private static int counter = 0;
		const int noOfRecordsPerFile = 20000;
		static int TotalRecords = 100000;

		private static void Main()
		{
			Stopwatch sw = new Stopwatch();
			var noOfIterations = TotalRecords / noOfRecordsPerFile;
			for (var i = 0; i < noOfIterations; i++)
			{
				var reportService = new ReportPdf();

				sw.Restart();
				var reportData = CreateReportData();
				var path = GetTempPdfPath();
				reportService.Export(path, reportData);
				sw.Stop();
				Console.WriteLine("Time taken" + sw.Elapsed.TotalSeconds);
				Process.Start(path);
			}
			sw.Reset();
			sw.Start();
			Console.WriteLine();
			ReportPdf.CombinePDFs(@"D:\GeneratedPDF", "ConcatenatedDocument1_tempfile.pdf");
			sw.Stop();
			Console.WriteLine("Time taken to combine the pdf" + sw.Elapsed.TotalSeconds);
			Console.ReadLine();
		}

		private static ReportData CreateReportData()
		{
			return new ReportData
			{
				Patient = new Patient
				{
					Id = "38561948",
					FirstName = "Daniel",
					LastName = "Price",
					Sex = Sex.Male,
					Birthdate = new DateTime(1950, 1, 1),
					Doctor = new Doctor
					{
						FirstName = "Heather",
						LastName = "Powell"
					}
				},
				StructureSet = GetStructureData()
			};
		}

		private static StructureSet GetStructureData()
		{
			var someObject = new StructureSet
			{
				Id = "CT",
				Image = new Image
				{
					Id = "CT_SCAN",
					CreationTime = new DateTime(2017, 9, 2, 15, 56, 12)
				},
				Structures = new[]
								{
						new Structure
						{
							Id = "PTV",
							VolumeInCc = 153.83,
							MeanDoseInGy = 47.12
						},
						new Structure
						{
							Id = "Bladder",
							VolumeInCc = 96.31,
							MeanDoseInGy = 38.60
						},
						new Structure
						{
							Id = "Bowel",
							VolumeInCc = 1683.98,
							MeanDoseInGy = 34.71,
						},
						new Structure
						{
							Id = "Femur, Left",
							VolumeInCc = 176.33,
							MeanDoseInGy = 15.45
						},
						new Structure
						{
							Id = "Femur, Right",
							VolumeInCc = 174.43,
							MeanDoseInGy = 16.11
						},
						new Structure
						{
							Id = "Prostate Bed",
							VolumeInCc = 76.67,
							MeanDoseInGy = 46.78,
						},
						new Structure
						{
							Id = "Rectum",
							VolumeInCc = 75.41,
							MeanDoseInGy = 26.39
						},
					}
			};

			var arr = new Structure[noOfRecordsPerFile];
			for (var i = start; i < noOfRecordsPerFile ; i++)
			{
				arr[i] = new Structure
				{
					Id = "PTV" + counter.ToString(),
					VolumeInCc = counter + 10,
					MeanDoseInGy = counter + 11
				};
				counter++;
			}
			someObject.Structures = arr;
			return someObject;
		}

		private static string GetTempPdfPath()
		{
			return Path.Combine(@"D:\GeneratedPDF", Guid.NewGuid().ToString() + ".pdf");
		}


	}
}
