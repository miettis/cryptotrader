<style lang="sass">
.my-sticky-virtscroll-table
  /* height or max-height is important */
  height: 675px

  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th /* bg color is important for th; just specify one */
    background-color: #ffffff

  thead tr th
    position: sticky
    z-index: 1
  /* this will be the loading indicator */
  thead tr:last-child th
    /* height of all previous header rows */
    top: 48px
  thead tr:first-child th
    top: 0

  /* prevent scrolling behind sticky top row on focus */
  tbody
    /* height of all previous header rows */
    scroll-margin-top: 48px
</style>
<template>
  <q-page class="items-center justify-evenly q-ma-sm">
    <q-table
      title="Orders"
      :rows="weekOrders"
      :columns="orderColumns"
      :pagination="orderPagination"
      virtual-scroll
      :rows-per-page-options="[0]"
      :virtual-scroll-sticky-size-start="48"
      row-key="id"
      class="q-ma-sm my-sticky-virtscroll-table"
    >
      <template v-slot:body-cell-averageFillPrice="props">
        <q-td :props="props">
          {{ props.row.averageFillPrice ? props.row.averageFillPrice : '' }}
          <template
            v-if="
              props.row.side == 'Sell' &&
              props.row.status === OrderStatus.Filled &&
              props.row.profit
            "
            ><span
              :class="props.row.profit > 0 ? 'text-green-10' : 'text-red-10'"
              >(<span v-if="props.row.profit > 0">+</span>
              {{ props.row.profit }}%)</span
            ></template
          >
        </q-td>
      </template>
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <q-btn
            color="primary"
            size="sm"
            class="q-ma-sm"
            @click="cancelOrder(props.row.id)"
            v-if="props.row.status === OrderStatus.New"
            >Cancel</q-btn
          >
        </q-td>
      </template>
    </q-table>

    <q-table
      title="Assets"
      :rows="assets"
      :columns="assetColumns"
      :pagination="assetPagination"
      row-key="symbol"
      class="q-ma-sm"
      @row-click="(evt, row, index) => changeSymbol(row.symbol)"
    >
      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th auto-width />
          <q-th v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.label }}
          </q-th>
        </q-tr>
      </template>
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td auto-width>
            <q-btn
              size="sm"
              color="accent"
              round
              dense
              @click="props.expand = !props.expand"
              :icon="props.expand ? 'remove' : 'add'"
            />
          </q-td>
          <q-td v-for="col in props.cols" :key="col.name" :props="props">
            <template v-if="col.name === 'symbol'">
              <q-btn
                flat
                color="primary"
                :label="col.value"
                @click="changeSymbol(props.row.symbol)"
              />
            </template>
            <template v-else-if="col.name === 'followed'">
              <q-toggle
                v-model="props.row.followed"
                @update:model-value="
                  (val, evt) => toggleFollowed(props.row.symbol, val)
                "
              />
            </template>
            <template v-else-if="col.name === 'trade'">
              <q-toggle
                v-model="props.row.trade"
                @update:model-value="
                  (val, evt) => toggleTrade(props.row.symbol, val)
                "
              />
            </template>
            <template v-else-if="col.name === 'actions'">
              <q-btn-dropdown
                color="primary"
                label="Sell"
                class="q-mr-sm"
                :disable="
                  props.row.symbol === 'USDT' || props.row.available === 0
                "
              >
                <q-list>
                  <q-item
                    clickable
                    v-close-popup
                    @click="sell(props.row.symbol, SellType.High)"
                  >
                    <q-item-section>
                      <q-item-label>Sell @high</q-item-label>
                    </q-item-section>
                  </q-item>
                  <q-item
                    clickable
                    v-close-popup
                    @click="sell(props.row.symbol, SellType.MA24Std)"
                  >
                    <q-item-section>
                      <q-item-label>Sell @MA+STD</q-item-label>
                    </q-item-section>
                  </q-item>
                  <q-item
                    clickable
                    v-close-popup
                    @click="sell(props.row.symbol, SellType.DefaultProfit)"
                  >
                    <q-item-section>
                      <q-item-label>Sell @DefaultProfit</q-item-label>
                    </q-item-section>
                  </q-item>
                </q-list>
              </q-btn-dropdown>
              <q-btn-dropdown
                color="primary"
                label="Buy"
                :disable="props.row.symbol === 'USDT'"
              >
                <q-list>
                  <q-item
                    clickable
                    v-close-popup
                    @click="startBuy(props.row.symbol, BuyType.Low)"
                  >
                    <q-item-section>
                      <q-item-label>Buy @low</q-item-label>
                    </q-item-section>
                  </q-item>
                  <q-item
                    clickable
                    v-close-popup
                    @click="startBuy(props.row.symbol, BuyType.MA24Std)"
                  >
                    <q-item-section>
                      <q-item-label>Buy @MA-STD</q-item-label>
                    </q-item-section>
                  </q-item>
                </q-list>
              </q-btn-dropdown>
            </template>
            <template v-else>
              {{ col.value }}
            </template>
          </q-td>
        </q-tr>
        <q-tr v-show="props.expand" :props="props">
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td>
            <div class="text-left">
              <div class="row">Low</div>
              <div class="row">{{ props.row.low }}</div>
              <div class="row">MA-Std</div>
              <div class="row">
                {{ props.row.ma?.sma - props.row.ma?.mad }}
              </div>
              <div class="row"></div>
              <div class="row">Purchase</div>
              <div class="row">
                {{ props.row.latestPurchasePrice }}
              </div>
            </div>
          </q-td>
          <q-td>
            <div class="text-left">
              <div class="row">High</div>
              <div class="row">{{ props.row.high }}</div>
              <div class="row">MA+Std</div>
              <div class="row">
                {{ props.row.ma?.sma + props.row.ma?.mad }}
              </div>
            </div>
          </q-td>
          <q-td> </q-td>
        </q-tr>
      </template>
      <template v-slot:bottom-row>
        <q-tr>
          <q-td><strong>Total</strong></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td></q-td>
          <q-td>
            {{ assets.reduce((acc, asset) => acc + (asset.valueLow ?? 0), 0) }}
          </q-td>
          <q-td>
            {{ assets.reduce((acc, asset) => acc + (asset.valueHigh ?? 0), 0) }}
          </q-td>
          <q-td></q-td>
        </q-tr>
      </template>
    </q-table>

    <q-card v-if="symbolDetails.symbol" class="q-ma-sm">
      <q-card-section>
        <div class="text-h6">{{ symbolDetails.symbol }}-USDT</div>
      </q-card-section>
      <q-card-section>
        <q-btn-group>
          <q-btn
            v-for="option in priceIntervalOptions"
            :key="option.value"
            @click="changePriceInterval(option.value)"
            :color="
              symbolDetails.priceInterval === option.value ? 'blue' : 'blue-3'
            "
            >{{ option.label }}</q-btn
          >
        </q-btn-group>
      </q-card-section>
      <q-card-section>
        <apexchart
          type="candlestick"
          :options="priceChartOptions"
          :series="priceSeries"
          ref="priceChart"
          height="500"
        ></apexchart>
        <apexchart
          type="bar"
          :options="returnChartOptions"
          :series="returnSeries"
          ref="returnChart"
          height="200"
        ></apexchart>
        <apexchart
          type="line"
          :options="featureChartOptions"
          :series="featureSeries"
          ref="featureChart"
          height="200"
        ></apexchart>

        <div class="row">
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="maFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="trendFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="momentumFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="cycleFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="volumeFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="volatilityFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
          <div class="col">
            <q-select
              filled
              v-model="symbolDetails.feature"
              :options="otherFeatures"
              @update:model-value="changeFeature"
              emit-value
              map-options
              style="min-width: 250px; max-width: 300px"
            />
          </div>
        </div>

        <apexchart
          type="line"
          :options="slopeChartOptions"
          :series="slopeSeries"
          ref="slopeChart"
          height="200"
        ></apexchart>
      </q-card-section>

      <q-separator dark />

      <q-card-actions>
        <q-btn-group>
          <q-btn
            v-for="option in slopeIntervalOptions"
            :key="option.value"
            @click="changeSlopeInterval(option.value)"
            :color="
              symbolDetails.slopeInterval === option.value ? 'blue' : 'blue-3'
            "
          >
            <span class="q-px-md">{{ option.label }}</span>
          </q-btn>
        </q-btn-group>
      </q-card-actions>
    </q-card>

    <q-dialog v-model="buyPromptVisible" persistent>
      <q-card style="min-width: 350px">
        <q-card-section>
          <div class="text-h6">USDT:</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-input
            dense
            v-model="buyDetails.usdt"
            autofocus
            @keyup.enter="confirmBuy"
          />
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn flat label="Cancel" v-close-popup />
          <q-btn flat label="Buy" @click="confirmBuy" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from 'vue';
import {
  AssetsClient,
  PricesClient,
  OrdersClient,
  AssetResponse,
  PriceHourResponse,
  OrderResponse,
  OrderStatus,
  BuyType,
  BuyRequest,
  SellType,
  SellRequest,
  IBuyRequest,
  SlopeHour,
} from 'src/api';
import { date } from 'quasar';
import { getApiUrl } from 'src/config';
import {
  getFeatureData,
  Feature,
  getTrendFeatures,
  getCycleFeatures,
  getMomentumFeatures,
  getVolumeFeatures,
  getVolatilityFeatures,
  getOtherFeatures,
  getAllFeatures,
  getMovingAverageFeatures,
} from 'src/data';

export interface SymbolDetails {
  symbol: string;
  priceInterval: number;
  slopeInterval: number;
  startDate: Date;
  endDate: Date;
  prices: PriceHourResponse[];
  orders: OrderResponse[];
  feature: Feature;
}

export interface BuyDetails {
  symbol: string;
  type: BuyType;
  usdt: number;
}

export interface Chart {
  clearAnnotations(): void;
  updateOptions(options: any): void;
  updateSeries(series: any[]): void;
  addYaxisAnnotation(annotation: any): void;
  addPointAnnotation(annotation: any): void;
}

export interface Series {
  name: string;
  data: any[];
  type: string;
}

export interface ApexChart {
  type: string;
  options: any;
  series: any[];
  ref: string;
  height: string;
}

export default defineComponent({
  name: 'TradePage',
  components: {},
  setup() {
    const { addToDate } = date;
    const apiUrl = getApiUrl();
    const assetsClient = new AssetsClient(apiUrl);
    const pricesClient = new PricesClient(apiUrl);
    const ordersClient = new OrdersClient(apiUrl);
    const maFeatures = getMovingAverageFeatures();
    const trendFeatures = getTrendFeatures();
    const cycleFeatures = getCycleFeatures();
    const momentumFeatures = getMomentumFeatures();
    const volumeFeatures = getVolumeFeatures();
    const volatilityFeatures = getVolatilityFeatures();
    const otherFeatures = getOtherFeatures();

    const colors = ['#2E93fA', '#66DA26', '#546E7A', '#E91E63', '#FF9800'];

    const priceIntervalOptions = [
      { label: 'Week', value: 7 },
      { label: '2 Weeks', value: 14 },
      { label: 'Month', value: 30 },
    ];

    const slopeIntervalOptions = [
      { label: '6h', value: 6 },
      { label: '8h', value: 8 },
      { label: '12h', value: 12 },
      { label: '24h', value: 24 },
    ];

    const assetColumns = [
      {
        name: 'symbol',
        required: true,
        label: 'Symbol',
        align: 'left',
        field: (row: AssetResponse) => row.symbol,
        format: (val: string) => `${val}`,
        sortable: true,
      },
      {
        name: 'followed',
        required: true,
        label: 'Followed',
        align: 'left',
        field: (row: AssetResponse) => row.followed,
        format: (val: boolean) => `${val}`,
        sortable: true,
      },
      {
        name: 'trade',
        required: true,
        label: 'Trade',
        align: 'left',
        field: (row: AssetResponse) => row.trade,
        format: (val: boolean) => `${val}`,
        sortable: true,
      },
      {
        name: 'available',
        required: true,
        label: 'Available',
        align: 'left',
        field: (row: AssetResponse) => row.available,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'total',
        required: true,
        label: 'Total',
        align: 'left',
        field: (row: AssetResponse) => row.total,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'currentValueMin',
        required: true,
        label: 'Value @low',
        align: 'left',
        field: (row: AssetResponse) => row.valueLow,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'currentValueMax',
        required: true,
        label: 'Value @high',
        align: 'left',
        field: (row: AssetResponse) => row.valueHigh,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'actions',
        required: true,
        label: '',
        align: 'left',
        sortable: false,
      },
    ];

    const assetPagination = {
      sortBy: 'total',
      descending: true,
      page: 1,
      rowsPerPage: 15,
    };

    const orderColumns = [
      {
        name: 'id',
        required: true,
        label: 'Id',
        align: 'left',
        field: (row: OrderResponse) => row.id,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'created',
        required: true,
        label: 'Created',
        align: 'left',
        field: (row: OrderResponse) => row.created,
        format: (val: Date) => `${val.toLocaleString('fi-FI')}`,
        sortable: true,
      },
      {
        name: 'updated',
        required: true,
        label: 'Updated',
        align: 'left',
        field: (row: OrderResponse) => row.updated,
        format: (val: Date) => `${val.toLocaleString('fi-FI')}`,
        sortable: true,
      },
      {
        name: 'side',
        required: true,
        label: 'Side',
        align: 'left',
        field: (row: OrderResponse) => row.side,
        format: (val: string) => `${val}`,
        sortable: true,
      },
      {
        name: 'symbol',
        required: true,
        label: 'Symbol',
        align: 'left',
        field: (row: OrderResponse) => row.symbol,
        format: (val: string) => `${val}`,
        sortable: true,
      },
      {
        name: 'status',
        required: true,
        label: 'Status',
        align: 'left',
        field: (row: OrderResponse) => row.status,
        format: (val: OrderStatus) => `${val}`,
        sortable: true,
      },
      {
        name: 'price',
        required: true,
        label: 'Price',
        align: 'left',
        field: (row: OrderResponse) => row.price,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'quantity',
        required: true,
        label: 'Quantity',
        align: 'left',
        field: (row: OrderResponse) => row.quantity,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'executedQuantity',
        required: true,
        label: 'Executed quantity',
        align: 'left',
        field: (row: OrderResponse) => row.executedQuantity,
        format: (val: number) => `${val}`,
        sortable: true,
      },
      {
        name: 'averageFillPrice',
        required: true,
        label: 'Fill price',
        align: 'left',
        field: (row: OrderResponse) => row.averageFillPrice,
        format: (val: number) => `${val ? val : ''}`,
        sortable: true,
      },
      {
        name: 'actions',
        required: true,
        label: '',
        align: 'left',
        sortable: false,
      },
    ];

    const orderPagination = {
      sortBy: 'updated',
      descending: true,
      //page: 1,
      rowsPerPage: 50,
    };

    const priceChartOptions = {
      chart: {
        id: 'price',
        //group: 'crypto',
        type: 'candlestick',
        height: '500',
      },
      title: {
        text: '',
        align: 'left',
      },
      xaxis: {
        type: 'datetime',
        labels: {
          show: false,
          datetimeUTC: true,
        },
      },
      yaxis: [
        {
          seriesName: 'price',
          title: {
            text: 'Price',
          },
          decimalsInFloat: 5,
          labels: {
            minWidth: 80,
          },
        },
      ],
      tooltip: {
        x: {
          format: 'dd.MM. HH:mm',
        },
      },
    };

    const returnChartOptions = {
      chart: {
        id: 'return',
        //group: 'crypto',
        type: 'bar',
        height: '200',
      },
      title: {
        text: '',
        align: 'left',
      },
      xaxis: {
        type: 'datetime',
      },
      yaxis: [
        {
          seriesName: 'return',
          title: {
            text: 'Return',
          },
          min: -0.4,
          max: 0.6,
          decimalsInFloat: 3,
          labels: {
            minWidth: 80,
          },
        },
      ],
      tooltip: {
        x: {
          format: 'dd.MM. HH:mm',
        },
      },
      stroke: {
        width: 2,
      },
      dataLabels: {
        enabled: false,
      },
    };

    const slopeChartOptions = {
      chart: {
        id: 'slope',
        //group: 'crypto',
        type: 'line',
        height: '200',
      },
      title: {
        text: '',
        align: 'left',
      },
      xaxis: {
        type: 'datetime',
      },
      yaxis: [
        {
          seriesName: 'slope',
          title: {
            text: 'Slope',
          },
          min: -1,
          max: 1,
          decimalsInFloat: 5,
          labels: {
            minWidth: 80,
          },
        },
      ],
      tooltip: {
        x: {
          format: 'dd.MM. HH:mm',
        },
      },
      stroke: {
        width: 2,
      },
    };

    const featureChartOptions = {
      chart: {
        id: 'feature',
        //group: 'crypto',
        type: 'line',
        height: '500',
      },
      title: {
        text: '',
        align: 'left',
      },
      xaxis: {
        type: 'datetime',
      },
      yaxis: [
        {
          seriesName: 'feature',
          title: {
            text: 'Feature',
          },
          min: 0,
          max: 1,
          decimalsInFloat: 3,
          labels: {
            minWidth: 80,
          },
        },
      ],
      tooltip: {
        x: {
          format: 'dd.MM. HH:mm',
        },
      },
      stroke: {
        width: 2,
      },
      dataLabels: {
        enabled: false,
      },
    };

    const priceSeries = reactive<Series[]>([
      { name: 'Price', data: [], type: 'candlestick' },
      { name: 'feature 1', data: [], type: 'line' },
      { name: 'feature 2', data: [], type: 'line' },
      { name: 'feature 3', data: [], type: 'line' },
      { name: 'feature 4', data: [], type: 'line' },
      { name: 'feature 5', data: [], type: 'line' },
    ]);
    const returnSeries = reactive<Series[]>([
      { name: 'Return', data: [], type: 'bar' },
    ]);
    const slopeSeries = reactive<Series[]>([
      { name: 'Slope', data: [], type: 'line' },
    ]);
    const featureSeries = reactive<Series[]>([
      { name: 'Feature', data: [], type: 'line' },
    ]);
    const assets = ref<AssetResponse[]>([]);
    const weekOrders = ref<OrderResponse[]>([]);

    const symbolDetails = ref<SymbolDetails>({
      symbol: '',
      priceInterval: 7,
      slopeInterval: 6,
      startDate: addToDate(new Date(), { days: -7 }),
      endDate: new Date(),
      prices: [],
      orders: [],
      feature: Feature.Aroon,
    });

    const getAssets = async () => {
      const response = await assetsClient.getAll();
      assets.value = response;
    };

    const getOrders = async () => {
      const start = addToDate(new Date(), { days: -30 });
      const response = await ordersClient.query('', start, new Date());
      weekOrders.value = response;
    };

    const priceChart = ref<Chart | null>(null);
    const returnChart = ref<Chart | null>(null);
    const slopeChart = ref<Chart | null>(null);
    const featureChart = ref<Chart | null>(null);
    const updateAnnotations = (orders: OrderResponse[]) => {
      if (priceChart.value) {
        priceChart.value.clearAnnotations();
        orders.forEach((order) => {
          let color = order.side === 'Buy' ? 'CornflowerBlue' : 'DarkOrchid';
          let price =
            order.status === OrderStatus.Filled
              ? order.averageFillPrice
              : order.price;

          if (order.status === OrderStatus.New) {
            priceChart.value?.addYaxisAnnotation({
              y: price,
              yAxisIndex: 0,
              borderColor: color,
              /*
              label: {
                text: order.side,
              },
              */
            });
          }
          if (
            order.status === OrderStatus.PartiallyFilled ||
            order.status === OrderStatus.Filled
          ) {
            let strokeColor =
              order.status === OrderStatus.Filled ? 'black' : '#bbbbbb';
            if (order.side === 'Buy') {
            } else {
              color = 'yellow';
            }
            priceChart.value?.addPointAnnotation({
              x: order.updated?.getTime() ?? 0,
              y: order.price,
              marker: {
                size: 3,
                shape: order.side === 'Buy' ? 'circle' : 'square',
                fillColor: color,
                strokeColor: strokeColor,
              },
              /*
              label: {
                text: order.side,
              },
              */
            });
          }
        });
      }
    };
    const refreshReturn = async () => {
      const returns: [number, number][] = [];

      symbolDetails.value.prices.forEach((price) => {
        if (price.return && price.return.day) {
          returns.push([
            price.timeOpen?.getTime() ?? 0,
            price.return?.day?.return ?? 0,
          ]);
        } else {
          returns.push([price.timeOpen?.getTime() ?? 0, 0]);
        }
      });

      if (returnChart.value) {
        //returnChart.value.updateOptions(slopeChartOptions);
        returnChart.value.updateSeries([
          { name: 'Return', data: returns, type: 'bar' },
        ]);
      } else {
        returnSeries.splice(0, 1, {
          name: 'Return',
          data: returns,
          type: 'bar',
        });
      }
    };
    const refreshSlope = async () => {
      const slope: [number, number][] = [];

      const slopeField: keyof SlopeHour =
        'high' + symbolDetails.value.slopeInterval;
      symbolDetails.value.prices.forEach((price) => {
        if (price.trend?.slope && price.trend.slope[slopeField] !== undefined) {
          if (price.timeOpen) {
            slope.push([
              price.timeOpen.getTime(),
              price.trend.slope[slopeField] as number,
            ]);
          }
        }
      });

      const slopeMax = symbolDetails.value.prices.reduce(
        (acc, price) =>
          Math.abs(price.trend?.slope?.[slopeField] ?? 0) > (acc ?? 0)
            ? Math.abs(price.trend?.slope?.[slopeField] ?? 0)
            : acc,
        0.01
      );

      slopeChartOptions.yaxis[0].min = -1.1 * slopeMax;
      slopeChartOptions.yaxis[0].max = 1.1 * slopeMax;

      if (slopeChart.value) {
        slopeChart.value.updateOptions(slopeChartOptions);
        slopeChart.value.updateSeries([
          { name: 'Slope', data: slope, type: 'line' },
        ]);
        slopeChart.value.clearAnnotations();
      } else {
        slopeSeries.splice(0, 1, { name: 'Slope', data: slope, type: 'line' });
      }

      setTimeout(() => {
        if (slopeChart.value) {
          slopeChart.value.addYaxisAnnotation({
            y: 0,
            strokeDashArray: 0,
            borderColor: 'red',
            borderWidth: 1,
            opacity: 1,
            yAxisIndex: priceChartOptions.yaxis.length - 1,
          });
        }
      }, 200);
    };
    /*
    const refreshFeature = async () => {

    };
    */
    const refreshSymbol = async (reloadData = false) => {
      if (reloadData) {
        const priceResponse = await pricesClient.queryHour(
          symbolDetails.value.symbol + 'USDT',
          symbolDetails.value.startDate,
          symbolDetails.value.endDate
        );
        const orderResponse = await ordersClient.query(
          symbolDetails.value.symbol + 'USDT',
          symbolDetails.value.startDate,
          symbolDetails.value.endDate
        );
        symbolDetails.value.prices = priceResponse;
        symbolDetails.value.orders = orderResponse;
      }
      const prices: [number, number[]][] = [];
      const predictHigh: [number, number][] = [];
      const predictLow: [number, number][] = [];
      const priceSeries2: {
        name: string;
        type: string;
        data: [number, number[]][] | [number, number][];
      }[] = [];

      const oscSeries: {
        name: string;
        type: string;
        data: [number, number][];
      }[] = [];

      symbolDetails.value.prices.forEach((price) => {
        prices.push([
          price.timeOpen?.getTime() ?? 0,
          [price.open ?? 0, price.high ?? 0, price.low ?? 0, price.close ?? 0],
        ]);
        if (price.predictionHigh ?? 0 > 0) {
          predictHigh.push([
            price.timeOpen?.getTime() ?? 0,
            price.predictionHigh ?? 0,
          ]);
        }
        if (price.predictionLow ?? 0 > 0) {
          predictLow.push([
            price.timeOpen?.getTime() ?? 0,
            price.predictionLow ?? 0,
          ]);
        }
      });
      let colorIndex = 0;
      priceSeries2.push({
        name: 'Price',
        data: prices,
        type: 'candlestick',
        color: '-',
      });

      if (predictHigh.length > 0) {
        priceSeries2.push({
          name: 'PredictionHigh',
          data: predictHigh,
          type: 'line',
          color: colors[colorIndex++],
        });
      }
      if (predictLow.length > 0) {
        priceSeries2.push({
          name: 'PredictionLow',
          data: predictLow,
          type: 'line',
          color: colors[colorIndex++],
        });
      }

      // populate feature data
      const featureData = getFeatureData(
        symbolDetails.value.prices,
        symbolDetails.value.feature
      );

      let secondaryMin = 0;
      let secondaryMax = 0;
      let oscMin = 0;
      let oscMax = 0;

      featureData.forEach((data) => {
        if (data.chart === 'price') {
          if (data.axis === 1) {
            priceSeries2.push({
              name: data.name,
              data: data.data,
              type: 'line',
              color: colors[colorIndex++],
            });
          } else {
            priceSeries2.push({
              name: data.name,
              data: data.data,
              type: 'line',
              opposite: true,
            });
            if (data.min < secondaryMin) {
              secondaryMin = data.min;
            }
            if (data.max > secondaryMax) {
              secondaryMax = data.max;
            }
          }
        } else if (data.chart === 'oscillator') {
          if (data.axis === 1) {
            oscSeries.push({ name: data.name, data: data.data, type: 'line' });
            if (data.min < oscMin) {
              oscMin = data.min;
            }
            if (data.max > oscMax) {
              oscMax = data.max;
            }
          } else {
            oscSeries.push({
              name: data.name,
              data: data.data,
              type: 'line',
              opposite: true,
            });
          }
        }
      });

      /*
      let min = featureData[0].min;
      let max = featureData[0].max;
      featureData.forEach((data) => {
        if (data.min < min) {
          min = data.min;
        }
        if (data.max > max) {
          max = data.max;
        }
      });
      */

      // adjust price chart secondary axis
      /*
      if (secondaryMin < 0) {
        priceChartOptions.yaxis[1].min = 1.1 * secondaryMin;
      } else {
        priceChartOptions.yaxis[1].min = 0.9 * secondaryMin;
      }
      if (secondaryMax < 0) {
        priceChartOptions.yaxis[1].max = 0.9 * secondaryMax;
      } else {
        priceChartOptions.yaxis[1].max = 1.1 * secondaryMax;
      }
      */
      // adjust feature chart primary axis
      if (oscMin < 0) {
        featureChartOptions.yaxis[0].min = 1.1 * oscMin;
      } else {
        featureChartOptions.yaxis[0].min = 0.9 * oscMin;
      }
      if (oscMax < 0) {
        featureChartOptions.yaxis[0].max = 0.9 * oscMax;
      } else {
        featureChartOptions.yaxis[0].max = 1.1 * oscMax;
      }

      if (priceChart.value) {
        priceChart.value.updateOptions(priceChartOptions);
        priceChart.value.updateSeries(priceSeries2);
        try {
          priceChart.value.clearAnnotations();
        } catch (e) {
          console.log(e);
        }
      } else {
        priceSeries.splice(0, priceSeries.length, priceSeries2);
      }

      if (featureChart.value) {
        featureChart.value.updateOptions(featureChartOptions);
        featureChart.value.updateSeries(oscSeries);
      } else {
        featureSeries.splice(0, featureSeries.length, oscSeries);
      }

      refreshReturn();
      //refreshFeature();
      //refreshSlope();

      setTimeout(() => {
        updateAnnotations(symbolDetails.value.orders);
      }, 500);
    };

    const changeSymbol = async (symbol: string) => {
      if (symbol === 'USDT') {
        return;
      }
      symbolDetails.value.symbol = symbol;
      refreshSymbol(true);
    };

    const changeFeature = async (feature: Feature) => {
      symbolDetails.value.feature = feature;
      //refreshFeature();
      refreshSymbol(false);
    };

    const changeSlopeInterval = async (hours: number) => {
      symbolDetails.value.slopeInterval = hours;
      await refreshSlope();
    };

    const changePriceInterval = async (interval: number) => {
      symbolDetails.value.priceInterval = interval;
      symbolDetails.value.startDate = addToDate(new Date(), {
        days: -interval,
      });
      symbolDetails.value.endDate = new Date();
      await changeSymbol(symbolDetails.value.symbol);
    };

    const cancelOrder = async (id: number) => {
      await ordersClient.cancel(id);
      await getAssets();
      await getOrders();
    };

    const buyDetails = ref<IBuyRequest>({
      symbol: '',
      type: BuyType.Low,
      usdt: 0,
    });

    const startBuy = async (symbol: string, type: BuyType) => {
      buyDetails.value.symbol = symbol + 'USDT';
      buyDetails.value.type = type;
      const usdtAsset = assets.value.find((asset) => asset.symbol === 'USDT');
      buyDetails.value.usdt = usdtAsset ? usdtAsset.available : 0;
      buyPromptVisible.value = true;
    };

    const confirmBuy = async () => {
      buyPromptVisible.value = false;
      if (buyDetails.value.usdt ?? 0 > 0) {
        await ordersClient.buy(new BuyRequest(buyDetails.value));
      }

      await getAssets();
      await getOrders();
    };

    const sell = async (symbol: string, type: SellType) => {
      var request = new SellRequest({
        symbol: symbol + 'USDT',
        type: type,
      });
      await ordersClient.sell(request);
      await getAssets();
      await getOrders();
    };

    const follow = async (symbol: string) => {
      await assetsClient.follow(symbol);
    };

    const toggleFollowed = async (symbol: string, enabled: boolean) => {
      if (enabled) {
        await assetsClient.follow(symbol);
      } else {
        await assetsClient.unfollow(symbol);
      }
    };

    const toggleTrade = async (symbol: string, enabled: boolean) => {
      if (enabled) {
        await assetsClient.enableTrade(symbol);
      } else {
        await assetsClient.disableTrade(symbol);
      }
    };

    const buyPromptVisible = ref(false);

    getAssets();
    getOrders();

    setInterval(async () => {
      await getAssets();
      await getOrders();
    }, 300000);

    setInterval(async () => {
      if (symbolDetails.value.symbol) {
        await changePriceInterval(symbolDetails.value.priceInterval);
      }
    }, 60000);

    return {
      // order table
      OrderStatus,
      orderColumns,
      orderPagination,
      weekOrders,

      // assets table
      assetColumns,
      assetPagination,
      assets,

      // selected symbol
      symbolDetails,

      // price chart
      priceChartOptions,
      priceSeries,
      priceChart,
      priceIntervalOptions,

      // return chart
      returnChartOptions,
      returnSeries,
      returnChart,

      // slope chart
      slopeChartOptions,
      slopeSeries,
      slopeChart,
      slopeIntervalOptions,

      // feature chart
      maFeatures,
      trendFeatures,
      cycleFeatures,
      momentumFeatures,
      volumeFeatures,
      volatilityFeatures,
      otherFeatures,
      changeFeature,
      featureChartOptions,
      featureSeries,
      featureChart,

      // Buy
      BuyType,
      startBuy,
      confirmBuy,
      buyPromptVisible,
      buyDetails,

      // Sell
      SellType,
      sell,

      getAssets,
      getOrders,
      changeSymbol,
      changeSlopeInterval,
      changePriceInterval,
      cancelOrder,

      follow,
      toggleFollowed,
      toggleTrade,
    };
  },
});
</script>
