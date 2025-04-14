const kpsChart = {
  template: `
    <div ref="chartContainer"></div>
  `,
  props: {
    data: {
      type: Object,
      required: true,
    },
  },
  mounted() {
    this.$nextTick(() => {
      this.initChart();
    });
  },
  watch: {
    data: {
      handler: 'renderChart',
      deep: true,
      immediate: false,
    }
  },
  methods: {
    initChart() {
      this.chart = echarts.init(this.$refs.chartContainer, 'dark');
      this.renderChart();
    },

    renderChart() {
      if (!this.chart || !this.data) {
        return;
      }

      const upData = this.data;
      const downData = this.data.map(value => -value);

      const option = {
        backgroundColor: 'transparent',
        grid: {
          top: 5,
          right: 5,
          bottom: 5,
          left: 30,
          containLabel: false
        },
        legend: {
          show: false
        },
        xAxis: {
          type: 'category',
          axisLabel: {
            show: false
          },
          axisTick: {
            show: false
          }
        },
        yAxis: {
          type: 'value',
          axisLabel: {
            margin: 2
          },
          scale: true,
          splitLine: {
            lineStyle: {
              opacity: 0.3
            }
          }
        },
        series: [
          {
            name: 'notes',
            type: 'bar',
            data: upData,
            itemStyle: {
              color: '#91cc75'
            },
            barWidth: '90%',
            barGap: '-100%'
          },
          {
            name: 'holds',
            type: 'bar',
            data: downData,
            itemStyle: {
              color: '#91cc75'
            },
            barWidth: '90%'
          }
        ]
      };

      this.chart.setOption(option);
    },
    beforeDestroy() {
      if (this.chart) {
        this.chart.dispose();
        this.chart = null;
      }
    }
  }
}

export default kpsChart;
