import { Component, OnInit, ViewChild } from '@angular/core';
import {
  ChartComponent,
  ApexChart,
  ApexAxisChartSeries,
  ApexTitleSubtitle,
  ApexDataLabels,
  ApexFill,
  ApexYAxis,
  ApexXAxis,
  ApexTooltip,
  ApexMarkers,
  ApexStroke,
  ApexGrid,
  ApexLegend,
  ApexPlotOptions,
  ApexResponsive,
  ApexNonAxisChartSeries
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexAxisChartSeries | ApexNonAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis | ApexYAxis[];
  title: ApexTitleSubtitle;
  labels: string[];
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  fill: ApexFill;
  tooltip: ApexTooltip;
  markers: ApexMarkers;
  grid: ApexGrid;
  legend: ApexLegend;
  plotOptions: ApexPlotOptions;
  responsive: ApexResponsive[];
  colors: string[];
};

@Component({
  selector: 'app-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss']
})
export class DefaultComponent implements OnInit {

  @ViewChild("chart") chart: ChartComponent;

  // 1. Evolution Orders (Bar Chart)
  public evolutionOrderOptions: Partial<ChartOptions>;

  // 2. Visits Trend (Line Chart)
  public visitsTrendOptions: Partial<ChartOptions>;

  // 3. Depot Distribution (Donut Chart)
  public depotDistributionOptions: Partial<ChartOptions>;

  // Sparklines for Top Cards
  public sparkline1Options: Partial<ChartOptions>;
  public sparkline2Options: Partial<ChartOptions>;
  public sparkline3Options: Partial<ChartOptions>;
  public sparkline4Options: Partial<ChartOptions>;

  // Data for lists
  public topProducts = [
    { name: 'Produit', count: 10, color: '#6610f2' }, // Purple
    { name: 'Produit', count: 8, color: '#0dcaf0' },  // Cyan
    { name: 'Produit', count: 6, color: '#ffc107' },  // Warning/Yellow
    { name: 'Produit 4', count: 6, color: '#fd3550' } // Red
  ];

  public topPharmacies = [
    { name: 'Pharmacie Ibn Jazzar', percent: 89, color: 'bg-primary' },
    { name: 'Pharmacie 2', percent: 76, color: 'bg-primary' },
    { name: 'Pharmacie 3', percent: 20, color: '#009688' }
  ];

  public delegateVisits = [
    { name: 'Ayoub', id: '99677180', count: 10, trend: '-2.24%', trendColor: 'text-danger', avatar: 'assets/images/avatars/avatar-1.png' },
    { name: 'Abdallah', id: '24617789', count: 5, trend: '-2.24%', trendColor: 'text-danger', avatar: 'assets/images/avatars/avatar-2.png' },
    { name: 'Amine', id: '23155677', count: 2, trend: '-2.24%', trendColor: 'text-danger', avatar: 'assets/images/avatars/avatar-3.png' },
    { name: 'Ahmed', id: '97866778', count: 1, trend: '-2.24%', trendColor: 'text-danger', avatar: 'assets/images/avatars/avatar-4.png' },
    { name: 'Aminee', id: '24158517', count: 1, trend: '-2.24%', trendColor: 'text-danger', avatar: 'assets/images/avatars/avatar-5.png' },
  ];

  public visitedPharmacies = [
    { name: 'Pharma 1', count: 10, color: '#6610f2' },
    { name: 'Pharma 22', count: 8, color: '#0dcaf0' },
    { name: 'Pharmacie 5', count: 6, color: '#ffc107' },
    { name: 'Pharma 4', count: 6, color: '#fd3550' }
  ];

  constructor() {
    this.initCharts();
  }

  ngOnInit(): void {
  }

  public evolutionFilter: string = 'Mois';

  initCharts() {
    this.updateEvolutionChart('Mois');
    
    // ... rest of initCharts logic handled via 'next' chunks or manual update if I were rewriting whole file.
    // However, since I am replacing a block, I need to include the rest ofinitCharts or be careful.
    // I will replace the START of initCharts and the evolutionOrderOptions definition.
    
    // 2. Visits Trend (Line Chart)
    this.visitsTrendOptions = {
      series: [{
        name: "Visites",
        data: [10, 25, 15, 30, 12, 35, 20, 41, 15, 25]
      }],
      chart: {
        height: 150,
        type: "area",
        sparkline: { enabled: true },
        toolbar: { show: false },
        fontFamily: 'Poppins, sans-serif'
      },
      stroke: {
        curve: "smooth",
        width: 3
      },
      fill: {
        type: "gradient",
        gradient: {
          shadeIntensity: 1,
          opacityFrom: 0.4,
          opacityTo: 0.1,
          stops: [0, 100]
        }
      },
      colors: ["#ff3e1d"],
      tooltip: {
        fixed: { enabled: false },
        x: { show: false },
        marker: { show: false }
      }
    };

    // 3. Depot Distribution (Donut Chart)
    this.depotDistributionOptions = {
      series: [27, 11, 22, 15, 25], // Percentages
      chart: {
        type: "donut",
        height: 320,
        fontFamily: 'Poppins, sans-serif'
      },
      labels: ["Produit 1", "Produit 2", "Produit 3", "Produit 4", "Others"],
      colors: ["#6f42c1", "#0d6efd", "#0dcaf0", "#ffc107", "#fd3550"],
      legend: {
        position: 'right',
        offsetY: 0,
        fontSize: '14px',
        itemMargin: {
            horizontal: 0,
            vertical: 10
        }
      },
      dataLabels: {
        enabled: true,
        formatter: function (val: any) {
             return Math.round(val) + "%";
        }
      },
      plotOptions: {
        pie: {
          donut: {
             size: '65%',
             labels: {
                show: true,
                total: {
                  show: true,
                  label: '11%', 
                  color: '#000', // Changed to black for visibility
                  fontSize: '24px',
                  fontWeight: 600,
                  formatter: function (w) {
                    return '11%';
                  }
                }
             }
          }
        }
      },
      responsive: [{
        breakpoint: 480,
        options: {
          legend: {
            position: 'bottom'
          }
        }
      }]
    };

    // Sparklines for top cards
    this.sparkline1Options = this.createSparkline([{ data: [5, 15, 10, 20, 15, 25, 20, 30, 25, 35] }], "#0d6efd"); // Blue
    this.sparkline2Options = this.createSparkline([{ data: [15, 25, 20, 30, 25, 35, 30, 40, 35, 45] }], "#fd7e14"); // Orange
    this.sparkline3Options = this.createSparkline([{ data: [45, 35, 40, 30, 35, 25, 30, 20, 25, 15] }], "#dc3545"); // Red
    this.sparkline4Options = this.createSparkline([{ data: [10, 20, 15, 25, 20, 30, 25, 35, 30, 40] }], "#a855f7"); // Purple
  }



  // Filter States
  // Filter States
  public filters = {
    evolution: 'Mois',
    topProducts: 'Mois',
    topPharmacies: 'Mois',
    delegateVisits: 'Mois',
    visitsTrend: 'Mois',
    visitedPharmacies: 'Mois',
    depotDistribution: 'Mois',
    sparkline1: 'Mois',
    sparkline2: 'Mois',
    sparkline3: 'Mois',
    sparkline4: 'Mois'
  };

  createSparkline(data: any[], color: string): Partial<ChartOptions> {
    return {
      series: data,
      chart: {
        type: "area",
        height: 50,
        sparkline: { enabled: true }
      },
      stroke: { curve: "smooth", width: 2 },
      fill: { type: "gradient", gradient: { shadeIntensity: 1, opacityFrom: 0.5, opacityTo: 0.0, stops: [0, 100] } },
      colors: [color],
      tooltip: {
        fixed: { enabled: false },
        x: { show: false },
        marker: { show: false },
        custom: function({series, seriesIndex, dataPointIndex, w}) {
            return '<div style="background-color: #fff; padding: 5px 10px; border: 1px solid #eee; border-radius: 5px; box-shadow: 0 0 10px rgba(0,0,0,0.1); color: ' + color + '; font-weight: 600;">' +
              series[seriesIndex][dataPointIndex] +
            '</div>';
        }
      }
    };
  }



  // Generic Filter Handler
  setFilter(section: string, range: string) {
    (this.filters as any)[section] = range;
    this.updateSectionData(section, range);
  }

  updateSectionData(section: string, range: string) {
    switch(section) {
      case 'evolution':
        this.updateEvolutionChart(range);
        break;
      case 'topProducts':
        this.updateTopProducts(range);
        break;
      case 'topPharmacies':
        this.updateTopPharmacies(range);
        break;
      case 'delegateVisits':
        this.updateDelegateVisits(range);
        break;
      case 'visitsTrend':
        this.updateVisitsTrend(range);
        break;
      case 'visitedPharmacies':
        this.updateVisitedPharmacies(range);
        break;
       case 'depotDistribution':
        break;
       case 'sparkline1':
        this.updateSparkline(1, range, '#0d6efd');
        break;
       case 'sparkline2':
        this.updateSparkline(2, range, '#fd7e14');
        break;
       case 'sparkline3':
        this.updateSparkline(3, range, '#dc3545');
        break;
       case 'sparkline4':
        this.updateSparkline(4, range, '#a855f7');
        break;
    }
  }

  updateSparkline(id: number, range: string, color: string) {
     let data = [5, 15, 10, 20, 15, 25, 20, 30, 25, 35]; // Default
     if (range === 'Année') data = [50, 40, 60, 50, 70, 60, 80, 70, 90, 80, 100, 90];
     if (range === 'Semaine') data = [10, 20, 15, 25, 20, 30, 25];

     const options = this.createSparkline([{ data: data }], color);
     
     if (id === 1) this.sparkline1Options = options;
     if (id === 2) this.sparkline2Options = options;
     if (id === 3) this.sparkline3Options = options;
     if (id === 4) this.sparkline4Options = options;
  }

  // --- Update Logics (Mock Data Changes) ---

  updateEvolutionChart(filter: string) {
    let categories: string[] = [];
    let data: number[] = [];
    let colors: string[] = [];

    if (filter === 'Semaine' || filter === 'Mois') { // Using 'Mois' as default view for "this month" breakdown
        categories = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        data = [4, 6, 12, 16, 8, 18, 6];
        colors = ["#ced4da", "#ced4da", "#ced4da", "#009688", "#ced4da", "#ced4da", "#ced4da"];
    } else if (filter === 'Année') {
        categories = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        // Sample data for Year
        data = [40, 60, 120, 160, 80, 180, 60, 90, 110, 130, 150, 200]; 
        colors = Array(12).fill("#ced4da");
        colors[5] = "#009688"; // Highlight June
    } else if (filter === 'Semestre') {
        categories = ["Mois 1", "Mois 2", "Mois 3", "Mois 4", "Mois 5", "Mois 6"];
        data = [150, 200, 180, 220, 250, 300];
        colors = ["#ced4da", "#ced4da", "#ced4da", "#009688", "#ced4da", "#ced4da"];
    } else if (filter === 'Custom') {
       categories = ["D1", "D2", "D3", "D4", "D5"];
       data = [10, 20, 15, 25, 30];
       colors = ["#ced4da", "#ced4da", "#009688", "#ced4da", "#ced4da"];
    }

    this.evolutionOrderOptions = {
      series: [{
        name: "Commandes",
        data: data
      }],
      chart: {
        type: "bar",
        height: 250,
        toolbar: { show: false },
        fontFamily: 'Poppins, sans-serif'
      },
      plotOptions: {
        bar: {
          columnWidth: "45%",
          borderRadius: 4,
        }
      },
      dataLabels: { enabled: false },
      stroke: {
        width: 0
      },
      grid: {
        show: false,
      },
      xaxis: {
        categories: categories,
        axisBorder: { show: false },
        axisTicks: { show: false },
        labels: {
          style: { colors: '#9ca3af' }
        }
      },
      yaxis: {
        show: false
      },
      colors: colors, 
      tooltip: {
        theme: 'dark'
      }
    };
  }

  updateTopProducts(range: string) {
    // Mocking data changes
    if (range === 'Année') {
        this.topProducts = [
            { name: 'Produit A (Year)', count: 120, color: '#6610f2' },
            { name: 'Produit B (Year)', count: 95, color: '#0dcaf0' },
            { name: 'Produit C (Year)', count: 80, color: '#ffc107' },
            { name: 'Produit D (Year)', count: 60, color: '#fd3550' }
        ];
    } else {
         this.topProducts = [
            { name: 'Produit 1', count: 10, color: '#6610f2' },
            { name: 'Produit 2', count: 8, color: '#0dcaf0' },
            { name: 'Produit 3', count: 6, color: '#ffc107' },
            { name: 'Produit 4', count: 6, color: '#fd3550' }
        ];
    }
  }

  updateTopPharmacies(range: string) {
      if (range === 'Année') {
          this.topPharmacies = [
            { name: 'Pharmacie Ibn Jazzar', percent: 95, color: 'bg-primary' },
            { name: 'Pharmacie 2', percent: 85, color: 'bg-primary' },
            { name: 'Pharmacie 3', percent: 60, color: 'bg-primary' }
          ];
      } else {
           this.topPharmacies = [
            { name: 'Pharmacie Ibn Jazzar', percent: 89, color: 'bg-primary' },
            { name: 'Pharmacie 2', percent: 76, color: 'bg-primary' },
            { name: 'Pharmacie 3', percent: 20, color: 'bg-primary' }
          ];
      }
  }

  updateDelegateVisits(range: string) {
      // Just shuffling counts for effect
      this.delegateVisits = this.delegateVisits.map(d => ({
          ...d,
          count: Math.floor(Math.random() * 20) + 1
      }));
  }

  updateVisitsTrend(range: string) {
      let data = [10, 25, 15, 30, 12, 35, 20, 41, 15, 25];
      if (range === 'Année') data = [100, 250, 150, 300, 120, 350, 200, 410, 150, 250, 300, 400];
      if (range === 'Semestre') data = [50, 80, 60, 90, 70, 100];
      
      this.visitsTrendOptions.series = [{ name: "Visites", data: data }];
  }

  updateVisitedPharmacies(range: string) {
      this.visitedPharmacies = this.visitedPharmacies.map(p => ({
          ...p,
          count: Math.floor(Math.random() * 15) + 5
      }));
  }

}
