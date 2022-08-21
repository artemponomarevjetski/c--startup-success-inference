using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StartupRating
{
    public partial class Form1 : Form
    {
        public System.Windows.Forms.DataVisualization.Charting.Series[] series_stack
            = new System.Windows.Forms.DataVisualization.Charting.Series[numberOfPoints];
        public const int numberOfPoints = 7;
        public string[] series_name = new string[numberOfPoints];
        public double[] pos = new double[numberOfPoints];
        private List<Startup> list_of_modeled_startups = new List<Startup>();
        public Form1()
        {
            InitializeComponent();
            int j = 0;
            series_name[j++] = "Staffing"; // 1
            series_name[j++] = "Finance"; // 2
            series_name[j++] = "Architecture"; // 3
            series_name[j++] = "Vendor"; // 4
            series_name[j++] = "Software"; // 5
            series_name[j++] = "Operations"; // 6
            series_name[j++] = "Business"; // 7
            for (int i = 0; i < numberOfPoints; i++)
            {
                pos[i] = 30.0;
            }
            chart1.Series["Startup"].Points.DataBindXY(series_name, pos);
            chart1.Click += new EventHandler(Mouse_Click);
            //
            for (int i = 0; i < numberOfPoints; i++)
            {
                series_stack[i] = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = series_name[i],
                    ChartArea = "Spartacus1",
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn
                };
                chart2.Series.Add(series_stack[i]);
            }
        }
        //
        private void Mouse_Click(object sender, EventArgs e)
        {
            Point point = chart1.PointToClient(System.Windows.Forms.Cursor.Position);
            int Xref = chart1.Location.X + 260, Yref = chart1.Location.Y + 376; // zzz -- how do these numbers depend on    
            //this.chart1.Location = new System.Drawing.Point(-31, -81);
            double Xnew = Convert.ToInt32(point.X - Xref);
            double Ynew = -Convert.ToInt32(point.Y - Yref);
            //     MessageBox.Show(Xnew.ToString());
            int sector = 0;
            double angle = 0.0, delta = 2.0 * Math.PI / Convert.ToDouble(numberOfPoints);
            if (Xnew != 0)
            {
                if (Xnew > 0.0 && Ynew > 0.0)
                {
                    angle = Math.Atan(Xnew / Ynew);
                }
                if (Xnew > 0.0 && Ynew < 0.0)
                {
                    angle = Math.PI + Math.Atan(Xnew / Ynew);
                }
                if (Xnew < 0.0 && Ynew < 0.0)
                {
                    angle = Math.PI + Math.Atan(Xnew / Ynew);
                }
                if (Xnew < 0.0 && Ynew > 0.0)
                {
                    angle = 2.0 * Math.PI + Math.Atan(Xnew / Ynew);
                }
            }
            else
            {
                angle = Math.PI / 2.0;
            }
            sector = Convert.ToInt32(angle / delta);
            if (sector == numberOfPoints)
            {
                sector = 0;
            }
            pos[sector] = Convert.ToInt32(Math.Sqrt((point.X - Xref) * (point.X - Xref)
                + (point.Y - Yref) * (point.Y - Yref)));
            pos[sector] /= 2.0; // /2 is to scale mouse click adjustment
            chart1.Series["Startup"].Points.Clear();
            chart1.Series["Startup"].Points.DataBindXY(series_name, pos);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Startup strtp = new Startup();
            for (int i = 0; i < numberOfPoints; i++)
            {
                strtp.pos[i] = pos[i];
            }
            chart1.Series["Startup"].Points.Clear();
            for (int i = 0; i < numberOfPoints; i++)
            {
                pos[i] = 30.0;
            }
            chart1.Series["Startup"].Points.DataBindXY(series_name, pos);
            chart1.Invalidate();
            list_of_modeled_startups.Add(strtp);
            for (int i = 0; i < list_of_modeled_startups.Count; i++)
            {
                list_of_modeled_startups[i].Spartacus_score = list_of_modeled_startups[i].SpartacusScore();
            }
            list_of_modeled_startups.Sort((x, y) => y.Spartacus_score.CompareTo(x.Spartacus_score));
            for (int i = 0; i < numberOfPoints; i++)
            {
                chart2.Series[series_name[i]].Points.Clear();
            }
            int j = 0;
            chart2.Series[series_name[j++]].Color = Color.Red; // 1
            chart2.Series[series_name[j++]].Color = Color.Blue; // 2
            chart2.Series[series_name[j++]].Color = Color.Pink; // 3
            chart2.Series[series_name[j++]].Color = Color.Yellow; // 4
            chart2.Series[series_name[j++]].Color = Color.DarkCyan; // 5
            chart2.Series[series_name[j++]].Color = Color.Aquamarine; // 6
            chart2.Series[series_name[j++]].Color = Color.Green; // 7 
            j = 0;
            foreach (Startup s in list_of_modeled_startups)
            {
                for (int i = 0; i < numberOfPoints; i++)
                {
                    chart2.Series[series_name[i]].Points.Add(new DataPoint(j, s.pos[i]));
                }
                j++;
            }
        }
        private partial class Startup
        {
            public double Spartacus_score = 0.0, Prometheus_score = 0.0, Atlas_score = 0.0;
            public double[] pos;
            public Startup()
            {
                pos = new double[numberOfPoints];
            }
            public double SpartacusScore()
            {
                double dtemp = 0.0;
                foreach (int i in pos)
                {
                    dtemp += Convert.ToDouble(i);
                }
                return dtemp;
            }
        }
    }
}

//#region Assembly System.Windows.Forms.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
//// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Windows.Forms.DataVisualization.dll
//#endregion

//using System.Collections;
//using System.ComponentModel;
//using System.ComponentModel.Design.Serialization;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Windows.Forms.DataVisualization.Charting.Utilities;

//namespace System.Windows.Forms.DataVisualization.Charting
//{
//    //
//    // Summary:
//    //     Serves as the root class of the System.Windows.Forms.DataVisualization.Charting.Chart
//    //     control.
//    [Designer("System.Windows.Forms.Design.DataVisualization.Charting.ChartWinDesigner, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
//    [DesignerSerializer("System.Windows.Forms.Design.DataVisualization.Charting.ChartWinDesignerSerializer, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//    [DisplayName("Chart")]
//    [SRDescriptionAttribute("DescriptionAttributeChart_Chart")]
//    [ToolboxBitmap(typeof(Chart), "ChartControl.ico")]
//    public class Chart : Control, ISupportInitialize, IDisposable
//    {
//        //
//        // Summary:
//        //     Initializes a new instance of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     class.
//        public Chart();

//        //
//        // Summary:
//        //     Gets or sets the size of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A System.Drawing.Size object that represents the size of the control.
//        [Bindable(true)]
//        [DefaultValue(typeof(Size), "300, 300")]
//        [SRCategoryAttribute("CategoryAttributeLayout")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_Size")]
//        public Size Size { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the text color of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A System.Drawing.Color value that specifies the text color of the chart.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DefaultValue(typeof(Color), "")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartColorEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeForeColor")]
//        [TypeConverter(typeof(ColorConverter))]
//        public override Color ForeColor { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the background color of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     object.
//        //
//        // Returns:
//        //     A System.Drawing.Color value used to draw the background color of the chart.
//        //     The default color is System.Drawing.Color.White.
//        [Bindable(true)]
//        [DefaultValue(typeof(Color), "White")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartColorEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBackColor")]
//        [TypeConverter(typeof(ColorConverter))]
//        public override Color BackColor { get; set; }
//        //
//        // Summary:
//        //     Gets a read-only System.Windows.Forms.DataVisualization.Charting.ChartAreaCollection
//        //     object that is used to store System.Windows.Forms.DataVisualization.Charting.ChartArea
//        //     objects.
//        //
//        // Returns:
//        //     A read-only System.Windows.Forms.DataVisualization.Charting.ChartAreaCollection
//        //     object that contains collection of System.Windows.Forms.DataVisualization.Charting.ChartArea
//        //     objects.
//        [Bindable(true)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartCollectionEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeChartAreas")]
//        public ChartAreaCollection ChartAreas { get; }
//        //
//        // Summary:
//        //     Gets or sets a flag that determines if a smooth gradient is applied when shadows
//        //     are drawn.
//        //
//        // Returns:
//        //     true if shadows are drawn using smoothing; otherwise, false. The default value
//        //     is true.
//        [Bindable(true)]
//        [DefaultValue(true)]
//        [SRCategoryAttribute("CategoryAttributeImage")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_SoftShadows")]
//        public bool IsSoftShadows { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality
//        //     type to use when applying anti-aliasing to text.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality enumeration
//        //     value used to apply anti-aliasing to text. The default value is System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.High.
//        [Bindable(true)]
//        [DefaultValue(typeof(TextAntiAliasingQuality), "High")]
//        [SRCategoryAttribute("CategoryAttributeImage")]
//        [SRDescriptionAttribute("DescriptionAttributeTextAntiAliasingQuality")]
//        public TextAntiAliasingQuality TextAntiAliasingQuality { get; set; }
//        //
//        // Summary:
//        //     Gets or sets a value that determines whether anti-aliasing is used when text
//        //     and graphics are drawn.
//        //
//        // Returns:
//        //     An System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles enumeration
//        //     value that determines whether anti-aliasing is used when text and graphics are
//        //     drawn.
//        [Bindable(true)]
//        [DefaultValue(typeof(AntiAliasingStyles), "All")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.FlagsEnumUITypeEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeImage")]
//        [SRDescriptionAttribute("DescriptionAttributeAntiAlias")]
//        public AntiAliasingStyles AntiAliasing { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the palette for the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartColorPalette enumeration
//        //     value that determines the palette to be used.
//        [Bindable(true)]
//        [DefaultValue(ChartColorPalette.BrightPastel)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ColorPaletteEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributePalette")]
//        public ChartColorPalette Palette { get; set; }
//        //
//        // Summary:
//        //     The System.Windows.Forms.DataVisualization.Charting.Chart.BackgroundImage property
//        //     is not used. Use the System.Windows.Forms.DataVisualization.Charting.Chart.BackImage
//        //     property instead.
//        //
//        // Returns:
//        //     An System.Drawing.Image object.
//        [Browsable(false)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        public override Image BackgroundImage { get; set; }
//        //
//        // Summary:
//        //     Gets or sets a collection that stores the chart annotations.
//        //
//        // Returns:
//        //     An System.Windows.Forms.DataVisualization.Charting.AnnotationCollection object
//        //     that is used to store chart annotations.
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.AnnotationCollectionEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeAnnotations3")]
//        public AnnotationCollection Annotations { get; }
//        //
//        // Summary:
//        //     Gets or sets a System.Windows.Forms.DataVisualization.Charting.TitleCollection
//        //     object that is used to store all System.Windows.Forms.DataVisualization.Charting.Title
//        //     objects used by the System.Windows.Forms.DataVisualization.Charting.Chart control.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.TitleCollection object that
//        //     is used to store all System.Windows.Forms.DataVisualization.Charting.Title objects
//        //     used by the chart control.
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartCollectionEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeTitles")]
//        public TitleCollection Titles { get; }
//        //
//        // Summary:
//        //     Gets or sets a System.Windows.Forms.DataVisualization.Charting.LegendCollection
//        //     that stores all System.Windows.Forms.DataVisualization.Charting.Legend objects
//        //     used by the System.Windows.Forms.DataVisualization.Charting.Chart control.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.LegendCollection object that
//        //     contains the System.Windows.Forms.DataVisualization.Charting.Legend objects used
//        //     by the chart.
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.LegendCollectionEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeLegends")]
//        public LegendCollection Legends { get; }
//        //
//        // Summary:
//        //     Gets a System.Windows.Forms.DataVisualization.Charting.SeriesCollection object,
//        //     which contains System.Windows.Forms.DataVisualization.Charting.Series objects.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.SeriesCollection object which
//        //     contains System.Windows.Forms.DataVisualization.Charting.Series objects.
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.SeriesCollectionEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_Series")]
//        public SeriesCollection Series { get; }
//        //
//        // Summary:
//        //     Gets a read-only System.Windows.Forms.DataVisualization.Charting.PrintingManager
//        //     object used for printing a chart.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.PrintingManager object used
//        //     for printing a chart.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_Printing")]
//        public PrintingManager Printing { get; }
//        //
//        // Summary:
//        //     Gets a System.Windows.Forms.DataVisualization.Charting.NamedImagesCollection
//        //     object that stores System.Windows.Forms.DataVisualization.Charting.NamedImage
//        //     objects for the chart.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.NamedImagesCollection object
//        //     that contains the collection of System.Windows.Forms.DataVisualization.Charting.NamedImage
//        //     objects.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [SRCategoryAttribute("CategoryAttributeChart")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_Images")]
//        public NamedImagesCollection Images { get; }
//        //
//        // Summary:
//        //     Gets a System.Windows.Forms.DataVisualization.Charting.DataManipulator object
//        //     that provides methods and properties that handle data.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.DataManipulator object that
//        //     provides methods and properties that handle data.
//        [Browsable(false)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeData")]
//        [SRDescriptionAttribute("DescriptionAttributeDataManipulator")]
//        public DataManipulator DataManipulator { get; }
//        //
//        // Summary:
//        //     Gets a System.Windows.Forms.DataVisualization.Charting.ChartSerializer object
//        //     that is used for chart serialization.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartSerializer object that
//        //     is used for chart serialization.
//        [Browsable(false)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeSerializer")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_Serializer")]
//        public ChartSerializer Serializer { get; }
//        //
//        // Summary:
//        //     Gets the font properties of the control.
//        //
//        // Returns:
//        //     A System.Drawing.Font object that represents the text font of the control.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 8pt")]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeCharttitle")]
//        public Font Font { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the hatching style of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle enumeration
//        //     that specifies the hatching style of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control. The default value is System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.None.
//        [Bindable(true)]
//        [DefaultValue(ChartHatchStyle.None)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.HatchStyleEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBackHatchStyle")]
//        public ChartHatchStyle BackHatchStyle { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the horizontal resolution of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     renderer.
//        //
//        // Returns:
//        //     A double value that represents the horizontal resolution of the chart renderer.
//        [Browsable(false)]
//        [DefaultValue(96)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeMisc")]
//        public double RenderingDpiX { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the vertical resolution of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     renderer.
//        //
//        // Returns:
//        //     A double value that represents the vertical resolution of the chart renderer.
//        [Browsable(false)]
//        [DefaultValue(96)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeMisc")]
//        public double RenderingDpiY { get; set; }
//        //
//        // Summary:
//        //     Gets the build number of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A string that represents the build number of the chart control.
//        [Browsable(false)]
//        [DefaultValue("")]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SRDescriptionAttribute("DescriptionAttributeChart_BuildNumber")]
//        public string BuildNumber { get; }
//        //
//        // Summary:
//        //     Gets or sets a System.Windows.Forms.DataVisualization.Charting.BorderSkin object,
//        //     which provides border skin functionality for the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.BorderSkin object which provides
//        //     border skin functionality for the chart.
//        [Bindable(true)]
//        [DefaultValue(BorderSkinStyle.None)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [NotifyParentProperty(true)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBorderSkin")]
//        [TypeConverter(typeof(LegendConverter))]
//        public BorderSkin BorderSkin { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the style of the border line.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartDashStyle enumeration
//        //     value that specifies the style of the border line.
//        [Bindable(true)]
//        [DefaultValue(ChartDashStyle.NotSet)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBorderDashStyle")]
//        public ChartDashStyle BorderlineDashStyle { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the width of the border line.
//        //
//        // Returns:
//        //     An integer value that specifies the width of the border line.
//        [Bindable(true)]
//        [DefaultValue(1)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_BorderlineWidth")]
//        public int BorderlineWidth { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the color of the border line.
//        //
//        // Returns:
//        //     A System.Drawing.Color value used to draw the border line.
//        [Bindable(true)]
//        [DefaultValue(typeof(Color), "White")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartColorEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBorderColor")]
//        [TypeConverter(typeof(ColorConverter))]
//        public Color BorderlineColor { get; set; }
//        //
//        // Summary:
//        //     Gets or sets a flag that determines whether non-critical exceptions should be
//        //     suppressed.
//        //
//        // Returns:
//        //     true if non-critical exceptions should be suppressed; otherwise, false. The default
//        //     value is false.
//        [DefaultValue(false)]
//        [SRCategoryAttribute("CategoryAttributeMisc")]
//        [SRDescriptionAttribute("DescriptionAttributeSuppressExceptions")]
//        public bool SuppressExceptions { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the style of the border.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartDashStyle enumeration
//        //     value that specifies the style of the border.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DefaultValue(ChartDashStyle.NotSet)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBorderDashStyle")]
//        public ChartDashStyle BorderDashStyle { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the color of the chart border.
//        //
//        // Returns:
//        //     A System.Drawing.Color value used to draw the chart border.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DefaultValue(typeof(Color), "White")]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartColorEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBorderColor")]
//        [TypeConverter(typeof(ColorConverter))]
//        public Color BorderColor { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the secondary color of the chart background.
//        //
//        // Returns:
//        //     A System.Drawing.Color value that represents the secondary color of the chart
//        //     background. The default value is System.Drawing.Color.Empty.
//        [Bindable(true)]
//        [DefaultValue(typeof(Color), "")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartColorEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBackSecondaryColor")]
//        [TypeConverter(typeof(ColorConverter))]
//        public Color BackSecondaryColor { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the orientation for the background gradient of a System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control. Also determines whether a gradient is used.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.GradientStyle enumeration that
//        //     specifies the orientation for the background gradient of a chart and whether
//        //     a gradient is used. The default value is System.Windows.Forms.DataVisualization.Charting.GradientStyle.None.
//        [Bindable(true)]
//        [DefaultValue(GradientStyle.None)]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.GradientEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBackGradientStyle")]
//        public GradientStyle BackGradientStyle { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the background image alignment used for the System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Unscaled
//        //     drawing mode.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle enumeration
//        //     value that specifies the background image alignment of the chart. The default
//        //     value is System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.TopLeft.
//        [Bindable(true)]
//        [DefaultValue(ChartImageAlignmentStyle.TopLeft)]
//        [NotifyParentProperty(true)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBackImageAlign")]
//        public ChartImageAlignmentStyle BackImageAlignment { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the color of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control that will be displayed as transparent.
//        //
//        // Returns:
//        //     A System.Drawing.Color value that will be displayed as transparent when the chart
//        //     image is drawn. The default value is System.Drawing.Color.Empty.
//        [Bindable(true)]
//        [DefaultValue(typeof(Color), "")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ChartColorEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [NotifyParentProperty(true)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeImageTransparentColor")]
//        [TypeConverter(typeof(ColorConverter))]
//        public Color BackImageTransparentColor { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the drawing mode for the background image of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode enumeration
//        //     value that specifies the drawing mode for the background image of the chart.
//        //     The default value is System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Tile.
//        [Bindable(true)]
//        [DefaultValue(ChartImageWrapMode.Tile)]
//        [NotifyParentProperty(true)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeImageWrapMode")]
//        public ChartImageWrapMode BackImageWrapMode { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the background image of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Returns:
//        //     A string value that represents the URL of an image file. The default value is
//        //     an empty string.
//        [Bindable(true)]
//        [DefaultValue("")]
//        [Editor("System.Windows.Forms.Design.DataVisualization.Charting.ImageValueEditor, System.Windows.Forms.DataVisualization.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
//        [NotifyParentProperty(true)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeBackImage")]
//        public string BackImage { get; set; }
//        //
//        // Summary:
//        //     Gets or set s the width of the chart border.
//        //
//        // Returns:
//        //     An integer value that determines the border width, in pixels, of the chart.
//        [Bindable(false)]
//        [Browsable(false)]
//        [DefaultValue(1)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_BorderlineWidth")]
//        public int BorderWidth { get; set; }
//        //
//        // Summary:
//        //     Gets or sets an array of custom palette colors.
//        //
//        // Returns:
//        //     An array of System.Drawing.Color objects that represent the set of colors used
//        //     for series on the chart.
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Attribute)]
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeChart_PaletteCustomColors")]
//        [TypeConverter(typeof(ColorArrayConverter))]
//        public Color[] PaletteCustomColors { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the data source for the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     object.
//        //
//        // Returns:
//        //     An System.Object that represents the data source for the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     object.
//        [AttributeProvider(typeof(IListSource))]
//        [Bindable(true)]
//        [DefaultValue(null)]
//        [SerializationVisibilityAttribute(Utilities.SerializationVisibility.Hidden)]
//        [SRCategoryAttribute("CategoryAttributeData")]
//        [SRDescriptionAttribute("DescriptionAttributeDataSource")]
//        public object DataSource { get; set; }
//        //
//        // Summary:
//        //     Gets the default control size.
//        //
//        // Returns:
//        //     A System.Drawing.Size object that represents the default size of the control.
//        protected override Size DefaultSize { get; }

//        //
//        // Summary:
//        //     Occurs when a numeric value has to be converted to a string.
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_PrePaint")]
//        public event EventHandler<FormatNumberEventArgs> FormatNumber;
//        //
//        // Summary:
//        //     Occurs when the cursor position is about to change.
//        [SRCategoryAttribute("CategoryAttributeCursor")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_CursorPositionChanging")]
//        public event EventHandler<CursorEventArgs> CursorPositionChanging;
//        //
//        // Summary:
//        //     Occurs when the cursor position is changed.
//        [SRCategoryAttribute("CategoryAttributeCursor")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_CursorPositionChanged")]
//        public event EventHandler<CursorEventArgs> CursorPositionChanged;
//        //
//        // Summary:
//        //     Occurs when the selection start or end position is about to change.
//        [SRCategoryAttribute("CategoryAttributeCursor")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_SelectionRangeChanging")]
//        public event EventHandler<CursorEventArgs> SelectionRangeChanging;
//        //
//        // Summary:
//        //     Occurs when the selection start position or end position is changed.
//        [SRCategoryAttribute("CategoryAttributeCursor")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_SelectionRangeChanged")]
//        public event EventHandler<CursorEventArgs> SelectionRangeChanged;
//        //
//        // Summary:
//        //     Occurs when the end-user places an annotation on the chart.
//        [SRCategoryAttribute("CategoryAttributeAnnotation")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AnnotationPlaced")]
//        public event EventHandler AnnotationPlaced;
//        //
//        // Summary:
//        //     Occurs when the axis scale view position or size is about to change.
//        [SRCategoryAttribute("CategoryAttributeAxisView")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AxisViewChanging")]
//        public event EventHandler<ViewEventArgs> AxisViewChanging;
//        //
//        // Summary:
//        //     Occurs when the axis scale view position or size is changed.
//        [SRCategoryAttribute("CategoryAttributeAxisView")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AxisViewChanged")]
//        public event EventHandler<ViewEventArgs> AxisViewChanged;
//        //
//        // Summary:
//        //     Occurs when the axis scroll bar is clicked by the end-user.
//        [SRCategoryAttribute("CategoryAttributeAxisView")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AxisScrollBarClicked")]
//        public event EventHandler<ScrollBarEventArgs> AxisScrollBarClicked;
//        //
//        // Summary:
//        //     Occurs when the chart element is painted.
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_PostPaint")]
//        public event EventHandler<ChartPaintEventArgs> PostPaint;
//        //
//        // Summary:
//        //     Occurs when the chart element background is painted.
//        [SRCategoryAttribute("CategoryAttributeAppearance")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_PrePaint")]
//        public event EventHandler<ChartPaintEventArgs> PrePaint;
//        //
//        // Summary:
//        //     Occurs just before the chart image is drawn. Use this event to customize the
//        //     chart picture.
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_Customize")]
//        public event EventHandler Customize;
//        //
//        // Summary:
//        //     Occurs when the chart legend must be customized.
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_CustomizeLegend")]
//        public event EventHandler<CustomizeLegendEventArgs> CustomizeLegend;
//        //
//        // Summary:
//        //     Occurs when the annotation text is changed.
//        [SRCategoryAttribute("CategoryAttributeAnnotation")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AnnotationTextChanged")]
//        public event EventHandler AnnotationTextChanged;
//        //
//        // Summary:
//        //     Occurs when a selection of the annotation is changed.
//        [SRCategoryAttribute("CategoryAttributeAnnotation")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AnnotationSelectionChanged")]
//        public event EventHandler AnnotationSelectionChanged;
//        //
//        // Summary:
//        //     Occurs when the annotation position is changed.
//        [SRCategoryAttribute("CategoryAttributeAnnotation")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AnnotationPositionChanged")]
//        public event EventHandler AnnotationPositionChanged;
//        //
//        // Summary:
//        //     Occurs when the annotation position is about to change.
//        [SRCategoryAttribute("CategoryAttributeAnnotation")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_AnnotationPositionChanging")]
//        public event EventHandler<AnnotationPositionChangingEventArgs> AnnotationPositionChanging;
//        //
//        // Summary:
//        //     Occurs before showing the tooltip to get the tooltip text.
//        [SRCategoryAttribute("CategoryAttributeToolTips")]
//        [SRDescriptionAttribute("DescriptionAttributeChartEvent_GetToolTipText")]
//        public event EventHandler<ToolTipEventArgs> GetToolTipText;

//        //
//        // Summary:
//        //     Aligns data points from different series along the X axis using their axis labels.
//        //     The specified series in the chart are aligned using an ascending sort order.
//        //
//        // Parameters:
//        //   series:
//        //     A comma-separated list of series that will have their data points aligned to
//        //     the X axis using the data point axis labels.
//        public void AlignDataPointsByAxisLabel(string series);
//        //
//        // Summary:
//        //     Aligns data points using their axis labels.
//        //
//        // Parameters:
//        //   series:
//        //     A comma-separated list of series that should be aligned by axis label.
//        //
//        //   sortingOrder:
//        //     A System.Windows.Forms.DataVisualization.Charting.PointSortOrder enumeration
//        //     value that determines if ascending or descending sort order is used on axis labels,
//        //     which in turn determines the order by which points that previously occupied the
//        //     same axis space are displayed.
//        public void AlignDataPointsByAxisLabel(string series, PointSortOrder sortingOrder);
//        //
//        // Summary:
//        //     Aligns data points using their axis labels. All series in the chart are aligned,
//        //     using the specified sort order.
//        //
//        // Parameters:
//        //   sortingOrder:
//        //     A System.Windows.Forms.DataVisualization.Charting.PointSortOrder object that
//        //     indicates if ascending or descending sort order is used on axis labels, which
//        //     in turn determines the order by which points that previously occupied the same
//        //     axis space are displayed.
//        public void AlignDataPointsByAxisLabel(PointSortOrder sortingOrder);
//        //
//        // Summary:
//        //     Aligns data points along the X axis using their axis labels. Applicable when
//        //     multiple series are indexed and their X-values are strings.
//        public void AlignDataPointsByAxisLabel();
//        //
//        // Summary:
//        //     Sets the automatically assigned series and data point colors, to allow programmatic
//        //     access at run time.
//        public void ApplyPaletteColors();
//        //
//        // Summary:
//        //     Signals to the object that initialization is starting.
//        public void BeginInit();
//        //
//        // Summary:
//        //     Data binds the System.Windows.Forms.DataVisualization.Charting.Chart control
//        //     to a data source.
//        public void DataBind();
//        //
//        // Summary:
//        //     Data binds a chart to the table, with one series created per unique value in
//        //     a given column.
//        //
//        // Parameters:
//        //   dataSource:
//        //     The data source that is data bound by a chart.
//        //
//        //   seriesGroupByField:
//        //     The name of the field used to group data into the series.
//        //
//        //   xField:
//        //     The name of the field for X values.
//        //
//        //   yFields:
//        //     A comma-separated list of name(s) of the field(s) for Y value(s).
//        //
//        //   otherFields:
//        //     The other data point properties that can be bound.
//        public void DataBindCrossTable(IEnumerable dataSource, string seriesGroupByField, string xField, string yFields, string otherFields);
//        //
//        // Summary:
//        //     Data binds a chart to the table, with one series created per unique value in
//        //     a given column.
//        //
//        // Parameters:
//        //   dataSource:
//        //     The data source that is data bound by a chart.
//        //
//        //   seriesGroupByField:
//        //     The name of the field used to group data into the series.
//        //
//        //   xField:
//        //     The name of the field for X values.
//        //
//        //   yFields:
//        //     A comma-separated list of name(s) of the field(s) for Y value(s).
//        //
//        //   otherFields:
//        //     The other data point properties that can be bound.
//        //
//        //   sortingOrder:
//        //     The order in which the series will be sorted by group field values.
//        public void DataBindCrossTable(IEnumerable dataSource, string seriesGroupByField, string xField, string yFields, string otherFields, PointSortOrder sortingOrder);
//        //
//        // Summary:
//        //     Automatically creates and binds series data to the specified data table, and
//        //     optionally populates X-values.
//        //
//        // Parameters:
//        //   dataSource:
//        //     The data source that is data bound by a chart, which can be any System.Collections.IEnumerable
//        //     object.
//        //
//        //   xField:
//        //     The name of the field used for the series X-values.
//        public void DataBindTable(IEnumerable dataSource, string xField);
//        //
//        // Summary:
//        //     Automatically creates and binds series data to the specified data table.
//        //
//        // Parameters:
//        //   dataSource:
//        //     The data source that is data bound by a chart, which can be any System.Collections.IEnumerable
//        //     object.
//        public void DataBindTable(IEnumerable dataSource);
//        //
//        // Summary:
//        //     Signals to the System.Windows.Forms.DataVisualization.Charting.Chart object that
//        //     initialization is complete.
//        public void EndInit();
//        //
//        // Summary:
//        //     Returns the chart element outline.
//        //
//        // Parameters:
//        //   element:
//        //     The System.Windows.Forms.DataVisualization.Charting.Chart object.
//        //
//        //   elementType:
//        //     The type of the element.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartElementOutline object
//        //     that contains:An array of points in absolute coordinates that can be used as
//        //     outline markers around this chart element. A System.Drawing.Drawing2D.GraphicsPath
//        //     object for drawing an outline around this chart element.
//        public ChartElementOutline GetChartElementOutline(object element, ChartElementType elementType);
//        //
//        // Summary:
//        //     Returns the requested chart service.
//        //
//        // Parameters:
//        //   serviceType:
//        //     The type of requested chart service.
//        //
//        // Returns:
//        //     An System.Object that represents the service type, or null if the service cannot
//        //     be found.
//        public object GetService(Type serviceType);
//        //
//        // Summary:
//        //     Determines the chart element, if any, that is located at a point defined by the
//        //     given X and Y coordinates.
//        //
//        // Parameters:
//        //   x:
//        //     The X-coordinate value of the point the user clicked.
//        //
//        //   y:
//        //     The Y-coordinate value of the point the user clicked.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.HitTestResult object, which
//        //     provides information concerning the chart element, if any, that is at the specified
//        //     location.
//        public HitTestResult HitTest(int x, int y);
//        //
//        // Summary:
//        //     Determines whether a chart element that is one of the specified types is located
//        //     at a point defined by the given X and Y coordinates.
//        //
//        // Parameters:
//        //   x:
//        //     The X-coordinate for the specified data point.
//        //
//        //   y:
//        //     The Y-coordinate for the specified data point.
//        //
//        //   ignoreTransparent:
//        //     true to ignore transparent elements; otherwise, false.
//        //
//        //   requestedElement:
//        //     An array of System.Windows.Forms.DataVisualization.Charting.ChartElementType
//        //     objects that specify the types to test for, in order to filter the result. If
//        //     omitted, checking for element types will be ignored and all element types will
//        //     be valid.
//        //
//        // Returns:
//        //     An array of System.Windows.Forms.DataVisualization.Charting.HitTestResult objects
//        //     that provides information about the chart element, if any, found at the specified
//        //     location. The array contains at least one element, which can be System.Windows.Forms.DataVisualization.Charting.ChartElementType.Nothing.
//        //     The objects in the result are sorted from the top to the bottom of different
//        //     layers of control.
//        public HitTestResult[] HitTest(int x, int y, bool ignoreTransparent, params ChartElementType[] requestedElement);
//        //
//        // Summary:
//        //     Determines if a chart element of a given type is located at a point defined by
//        //     given X and Y coordinates.
//        //
//        // Parameters:
//        //   x:
//        //     The X-coordinate value of the point the user clicked on.
//        //
//        //   y:
//        //     The Y-coordinate value of the point the user clicked on.
//        //
//        //   requestedElement:
//        //     A flag that determines the chart element type to be tested.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.HitTestResult object, which
//        //     provides information concerning the chart element, if any, that is at the specified
//        //     location.
//        public HitTestResult HitTest(int x, int y, ChartElementType requestedElement);
//        //
//        // Summary:
//        //     Determines the chart element, if any, that is located at a point defined by given
//        //     X and Y coordinates. Transparent elements can optionally be ignored.
//        //
//        // Parameters:
//        //   x:
//        //     The X-coordinate value of the point the user clicked on.
//        //
//        //   y:
//        //     The Y-coordinate value of the point the user clicked on.
//        //
//        //   ignoreTransparent:
//        //     true to ignore transparent elements; otherwise, false.
//        //
//        // Returns:
//        //     A System.Windows.Forms.DataVisualization.Charting.HitTestResult object, which
//        //     provides information concerning the chart element, if any, that is at the specified
//        //     location.
//        public HitTestResult HitTest(int x, int y, bool ignoreTransparent);
//        //
//        // Summary:
//        //     Invalidates the specified area of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        //
//        // Parameters:
//        //   rectangle:
//        //     A System.Drawing.Rectangle object that specifies the area to invalidate.
//        public void Invalidate(Rectangle rectangle);
//        //
//        // Summary:
//        //     Invalidates the entire surface of the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     and causes the System.Windows.Forms.DataVisualization.Charting.Chart control
//        //     to be redrawn.
//        public void Invalidate();
//        //
//        // Summary:
//        //     Loads a template with the specified filename from the disk.
//        //
//        // Parameters:
//        //   name:
//        //     The file name of the template to load. You must specify the full path of the
//        //     template to be loaded from the disk.
//        public void LoadTemplate(string name);
//        //
//        // Summary:
//        //     Loads a template into the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control from an image stream.
//        //
//        // Parameters:
//        //   stream:
//        //     The stream to load from.
//        public void LoadTemplate(Stream stream);
//        //
//        // Summary:
//        //     Forces the control to invalidate its client area and immediately redraw itself
//        //     and any child controls.
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        public override void Refresh();
//        //
//        // Summary:
//        //     Resets automatically calculated chart property values to "Auto".
//        public void ResetAutoValues();
//        //
//        // Summary:
//        //     Saves the chart image to the specified stream.
//        //
//        // Parameters:
//        //   imageStream:
//        //     The stream in which the image is saved to.
//        //
//        //   format:
//        //     The chart image format.
//        public void SaveImage(Stream imageStream, ChartImageFormat format);
//        //
//        // Summary:
//        //     Saves the image to the specifed stream.
//        //
//        // Parameters:
//        //   imageStream:
//        //     The stream in which the image is saved to.
//        //
//        //   format:
//        //     The image format.
//        public void SaveImage(Stream imageStream, ImageFormat format);
//        //
//        // Summary:
//        //     Saves the chart image to the specified file.
//        //
//        // Parameters:
//        //   imageFileName:
//        //     The name of the file in which image is saved to.
//        //
//        //   format:
//        //     The chart image format.
//        public void SaveImage(string imageFileName, ChartImageFormat format);
//        //
//        // Summary:
//        //     Saves an image to the specified file.
//        //
//        // Parameters:
//        //   imageFileName:
//        //     The name of the file in which image is saved to.
//        //
//        //   format:
//        //     The image format.
//        public void SaveImage(string imageFileName, ImageFormat format);
//        //
//        // Summary:
//        //     Updates the annotations in the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        public void UpdateAnnotations();
//        //
//        // Summary:
//        //     Updates the cursor in the System.Windows.Forms.DataVisualization.Charting.Chart
//        //     control.
//        public void UpdateCursor();
//        //
//        // Summary:
//        //     When overridden in a derived class, returns the custom System.Windows.Forms.AccessibleObject
//        //     for the entire chart.
//        //
//        // Returns:
//        //     The System.Windows.Forms.AccessibleObject for the chart.
//        protected override AccessibleObject CreateAccessibilityInstance();
//        //
//        // Summary:
//        //     Releases unmanaged and, optionally, managed resources.
//        //
//        // Parameters:
//        //   disposing:
//        //     true to release both unmanaged and managed resources; false to release only unmanaged
//        //     resources.
//        protected override void Dispose(bool disposing);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnCursorChanged(System.EventArgs)
//        //     method and raises the System.Windows.Forms.DataVisualization.Charting.Chart.CursorPositionChanging
//        //     and System.Windows.Forms.DataVisualization.Charting.Chart.CursorPositionChanged
//        //     events.
//        //
//        // Parameters:
//        //   e:
//        //     An System.EventArgs that contains the event data.
//        protected override void OnCursorChanged(EventArgs e);
//        //
//        // Summary:
//        //     Raises the System.Windows.Forms.DataVisualization.Charting.Chart.Customize event.
//        [SRDescriptionAttribute("DescriptionAttributeChart_OnCustomize")]
//        protected virtual void OnCustomize();
//        //
//        // Summary:
//        //     Raises the System.Windows.Forms.DataVisualization.Charting.Chart.CustomizeLegend
//        //     event.
//        //
//        // Parameters:
//        //   legendItems:
//        //     A System.Windows.Forms.DataVisualization.Charting.LegendItemsCollection object.
//        //
//        //   legendName:
//        //     The name of the legend.
//        [SRDescriptionAttribute("DescriptionAttributeChart_OnCustomizeLegend")]
//        protected virtual void OnCustomizeLegend(LegendItemsCollection legendItems, string legendName);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnDoubleClick(System.EventArgs) method.
//        //
//        // Parameters:
//        //   e:
//        //     An System.EventArgs object that contains the event arguments.
//        protected override void OnDoubleClick(EventArgs e);
//        //
//        // Summary:
//        //     Raises the System.Windows.Forms.DataVisualization.Charting.Chart.FormatNumber
//        //     event.
//        //
//        // Parameters:
//        //   e:
//        //     A System.Windows.Forms.DataVisualization.Charting.FormatNumberEventArgs object
//        //     that contains the event arguments.
//        protected virtual void OnFormatNumber(FormatNumberEventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnLocationChanged(System.EventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     An System.EventArgs object that contains the event arguments.
//        protected override void OnLocationChanged(EventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     A System.MouseEventArgs object that contains the event arguments.
//        protected override void OnMouseDown(MouseEventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     A System.MouseEventArgs object that contains the event arguments.
//        protected override void OnMouseMove(MouseEventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnMouseUp(System.Windows.Forms.MouseEventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     A System.MouseEventArgs object that contains the event arguments.
//        protected override void OnMouseUp(MouseEventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnPaint(System.Windows.Forms.PaintEventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     A System.PaintEventArgs object that contains the event arguments.
//        protected override void OnPaint(PaintEventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnPaintBackground(System.Windows.Forms.PaintEventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   pevent:
//        //     A System.PaintEventArgs object that contains the event arguments.
//        protected override void OnPaintBackground(PaintEventArgs pevent);
//        //
//        // Summary:
//        //     Raises the System.Windows.Forms.DataVisualization.Charting.Chart.PostPaint event.
//        //
//        // Parameters:
//        //   e:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs object
//        //     that contains the event arguments.
//        protected virtual void OnPostPaint(ChartPaintEventArgs e);
//        //
//        // Summary:
//        //     Raises the System.Windows.Forms.DataVisualization.Charting.Chart.PrePaint event.
//        //
//        // Parameters:
//        //   e:
//        //     A System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs object
//        //     that contains the event arguments.
//        protected virtual void OnPrePaint(ChartPaintEventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnResize(System.EventArgs) method.
//        //
//        // Parameters:
//        //   e:
//        //     An System.EventArgs object that contains the event arguments.
//        protected override void OnResize(EventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnRightToLeftChanged(System.EventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     An System.EventArgs object that contains the event arguments.
//        protected override void OnRightToLeftChanged(EventArgs e);
//        //
//        // Summary:
//        //     Overrides the System.Windows.Forms.Control.OnSystemColorsChanged(System.EventArgs)
//        //     method.
//        //
//        // Parameters:
//        //   e:
//        //     An System.EventArgs that contains the event data.
//        protected override void OnSystemColorsChanged(EventArgs e);
//    }
//}