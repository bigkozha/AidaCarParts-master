<template>
    <v-app id="inspire" flat>
        <v-app-bar app color="white">
            <v-container class="py-0 fill-height">
                <v-avatar class="mr-10" color="grey darken-1" size="32"></v-avatar>
                <v-menu v-for="[text, section] in menu"
                        :key="text"
                        :rounded="'lg'"
                        offset-y>
                    <template v-slot:activator="{ attrs, on }">
                        <v-btn color="black"
                               class="white--text ma-1"
                               v-bind="attrs"
                               v-on="on"
                               small>
                            {{ text }}
                        </v-btn>
                    </template>
                    <v-list>
                        <v-list-item v-for="item in section"
                                     :key="item.index"
                                     link
                                     @click="changeCategory(item.index)">
                            <v-list-item-title v-text="item.name"></v-list-item-title>
                        </v-list-item>
                    </v-list>
                </v-menu>
                <v-spacer></v-spacer>
                <v-btn color="orange"
                       class="white--text ma-1"
                       @click="createClicked"
                       small>
                    Создать
                </v-btn>
                <v-responsive max-width="260">
                    <v-text-field dense
                                  flat
                                  hide-details
                                  rounded
                                  solo-inverted
                                  text
                                  v-model="searchWord"
                                  append-icon="mdi-search-web"
                                  @click:append="search"
                                  @keydown.enter="search">
                    </v-text-field>
                </v-responsive>
            </v-container>
        </v-app-bar>

        <v-main class="grey lighten-3">
            <v-item-group multiple>
                <v-container>
                    <v-layout row wrap>
                        <v-flex xs12
                                md3
                                v-for="n in carPartItems"
                                :key="n.id"
                                class="pa-1 ma-1">
                            <CarPartCard v-bind:id="n.id"
                                         v-bind:partName="n.partName"
                                         v-bind:note="n.note"
                                         v-bind:unitOfMeasure="n.unitOfMeasure"
                                         v-bind:volume="n.volume"
                                         v-bind:availibility="n.availibility"
                                         v-bind:cost="n.cost"
                                         v-bind:oem="n.oem"
                                         v-bind:section="n.section"
                                         v-bind:subsection="n.subsection"
                                         v-bind:costNumber="n.costNumber"
                                         v-bind:picSrc="n.picSrc"
                                         v-bind:partCode="n.partCode"
                                         v-bind:weight="n.weight"
                                         @editClicked="editClicked">
                            </CarPartCard>
                        </v-flex>
                    </v-layout>
                    <v-pagination v-model="page"
                                  :length="carPartItemsLength"
                                  total-visible="7"
                                  next-icon="mdi-menu-right"
                                  prev-icon="mdi-menu-left"
                                  color="black"
                                  @input="changeCurrentPage"></v-pagination>
                </v-container>
            </v-item-group>
            <v-dialog v-model="dialog" persistent max-width="600px" v-if="isAdmin">
                <v-card>
                    <v-card-title>
                        <span class="headline">Редактирование</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field label="Название"
                                                  v-model="editItem.partName"
                                                  required></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Применение"
                                                  v-model="editItem.note"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Код товара"
                                                  v-model="editItem.partCode"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="OEM"
                                                  v-model="editItem.oem"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Ед. измерения"
                                                  v-model="editItem.unitOfMeasure"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Обьем"
                                                  v-model="editItem.volume"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Ед. измерения"
                                                  v-model="editItem.unitOfMeasure"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Вес"
                                                  v-model="editItem.weight"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6" md="4">
                                    <v-text-field label="Доступность"
                                                  v-model="editItem.availibility"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6">
                                    <v-autocomplete v-model="editItem.sectionAndSubsectionId"
                                                    :items="categories()"
                                                    item-text="name"
                                                    item-value="index"
                                                    label="Категория"></v-autocomplete>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-btn color="red darken-1" text @click="deletePartById" v-if="!isCreate">
                            Удалить
                        </v-btn>
                        <v-spacer></v-spacer>
                        <v-btn color="blue darken-1" text @click="editItemById" v-if="!isCreate">
                            Сохранить
                        </v-btn>
                        <v-btn color="blue darken-1" text @click="createPart" v-if="isCreate">
                            Создать
                        </v-btn>
                        <v-btn color="blue darken-1" text @click="closeClicked">
                            Закрыть
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-main>
    </v-app>
</template>

<script>
    import CarPartCard from "../components/CarPartCard";
    export default {
        components: {
            CarPartCard,
        },
        data: () => ({
            page: 1,
            sectionIndex: 2,
            searchWord: null,
            localCarPartItems: [],
            dialog: false,
            isCreate: false,
            menu: [
                [
                    "Двигатель",
                    [
                        {
                            name: "Фильтры",
                            index: 2,
                        },
                        {
                            name: "Аксессуары",
                            index: 7,
                        },
                        {
                            name: "ДВС",
                            index: 9,
                        },
                        {
                            name: "Питание",
                            index: 10,
                        },
                    ],
                ],
                [
                    "КПП",
                    [
                        {
                            name: "КПП",
                            index: 16,
                        },
                    ],
                ],
                [
                    "Разное",
                    [
                        {
                            name: "Универсальные",
                            index: 1,
                        },
                        {
                            name: "Патрубки",
                            index: 15,
                        },
                        {
                            name: "Ремни",
                            index: 8,
                        },
                        {
                            name: "Уплотнения",
                            index: 6,
                        },
                        {
                            name: "Отопление",
                            index: 21,
                        },
                    ],
                ],
                [
                    "Тормоза",
                    [
                        {
                            name: "Пневматика",
                            index: 3,
                        },
                        {
                            name: "Механика",
                            index: 17,
                        },
                    ],
                ],
                [
                    "Шасси",
                    [
                        {
                            name: "Кузов",
                            index: 18,
                        },
                        {
                            name: "Подвеска",
                            index: 12,
                        },
                        {
                            name: "Гидравлика",
                            index: 13,
                        },
                        {
                            name: "Рулевое",
                            index: 14,
                        },
                        {
                            name: "Мосты",
                            index: 4,
                        },
                        {
                            name: "Подшипники",
                            index: 20,
                        },
                    ],
                ],
                [
                    "Экстерьер",
                    [
                        {
                            name: "Экстерьер",
                            index: 19,
                        },
                    ],
                ],
                [
                    "Электрика",
                    [
                        {
                            name: "Электрика",
                            index: 11,
                        },
                    ],
                ],
                [
                    "Все категории",
                    [
                        {
                            name: "Все категории",
                            index: null,
                        },
                    ],
                ],
            ],
            editItem: {
                id: null,
                partName: null,
                note: null,
                unitOfMeasure: null,
                volume: null,
                weigth: null,
                availibility: null,
                cost: null,
                oem: null,
                section: null,
                subsection: null,
                costNumber: null,
                picSrc: null,
                partCode: null,
            },
        }),
        computed: {
            carPartItems() {
                return this.$store.state.carPartItems;
            },
            isAdmin() {
                return this.$store.getters.isLoggedIn;
            },
            carPartItemsLength() {
                return this.$store.getters.carPartItemsLength;
            }
        },
        mounted() {
            this.changeCurrentPage();
        },
        methods: {
            changeCurrentPage() {
                this.$store.dispatch("getPartsByPageNumber", {
                    page: this.page - 1,
                    sectionIndex: this.sectionIndex,
                    searchWord: this.searchWord,
                });
            },
            changeCategory(index) {
                this.sectionIndex = index;
                this.page = 1;
                this.searchWord = null;
                this.changeCurrentPage();
            },
            search() {
                this.page = 1;
                this.sectionIndex = null;
                this.changeCurrentPage();
            },
            editClicked(id) {
                this.editItem = this.carPartItems.find((p) => p.id === id);
                this.dialog = !this.dialog;
                this.isCreate = false;
            },
            createPart() {
                this.$store.dispatch('createPart', {
                    id: this.editItem.id,
                    partName: this.editItem.partName,
                    note: this.editItem.note,
                    unitOfMeasure: this.editItem.unitOfMeasure,
                    volume: this.editItem.volume,
                    weigth: this.editItem.weigth,
                    availibility: this.editItem.availibility,
                    cost: this.editItem.cost,
                    oem: this.editItem.oem,
                    section: this.editItem.section,
                    subsection: this.editItem.subsection,
                    costNumber: this.editItem.costNumber,
                    picSrc: this.editItem.picSrc,
                    partCode: this.editItem.partCode,
                    sectionAndSubsectionId: this.editItem.sectionAndSubsectionId,
                }).then(() => {
                    this.changeCurrentPage();
                    this.dialog = false;
                    this.isCreate = false;
                })
                    .catch(e => console.log(e))
            },
            editItemById() {
                let partToUpdate = this.editItem;
                console.log(partToUpdate)
                this.$store
                    .dispatch("updatePart", {
                        id: partToUpdate.id,
                        partName: partToUpdate.partName,
                        note: partToUpdate.note,
                        unitOfMeasure: partToUpdate.unitOfMeasure,
                        volume: partToUpdate.volume,
                        weigth: partToUpdate.weigth,
                        availibility: partToUpdate.availibility,
                        cost: partToUpdate.cost,
                        oem: partToUpdate.oem,
                        section: partToUpdate.section,
                        subsection: partToUpdate.subsection,
                        costNumber: partToUpdate.costNumber,
                        picSrc: partToUpdate.picSrc,
                        partCode: partToUpdate.partCode,
                        sectionAndSubsectionId: partToUpdate.sectionAndSubsectionId,
                    })
                    .then(() => {
                        this.editItem = {
                            id: null,
                            partName: null,
                            note: null,
                            unitOfMeasure: null,
                            volume: null,
                            weigth: null,
                            availibility: null,
                            cost: null,
                            oem: null,
                            section: null,
                            subsection: null,
                            costNumber: null,
                            picSrc: null,
                            partCode: null,
                        };
                        this.dialog = false;
                    }).catch((e) => console.log(e));
            },
            deletePartById() {
                this.$store.dispatch('deletePartById', {
                    id: this.editItem.id,
                }).then(() => {
                    this.editItem = {
                        id: null,
                        partName: null,
                        note: null,
                        unitOfMeasure: null,
                        volume: null,
                        weigth: null,
                        availibility: null,
                        cost: null,
                        oem: null,
                        section: null,
                        subsection: null,
                        costNumber: null,
                        picSrc: null,
                        partCode: null,
                    };
                    this.dialog = false;
                }).catch((e) => console.log(e));
            },
            categories() {
                return this.menu
                    .map((item) => item[1].map((item) => item))
                    .flat()
                    .filter((i) => i.index !== null);
            },
            createClicked() {
                this.editItem = {
                    id: null,
                    partName: null,
                    note: null,
                    unitOfMeasure: null,
                    volume: null,
                    weigth: null,
                    availibility: null,
                    cost: null,
                    oem: null,
                    section: null,
                    subsection: null,
                    costNumber: null,
                    picSrc: null,
                    partCode: null,
                };
                this.isCreate = true;
                this.dialog = true;
            },
            closeClicked() {
                this.isCreate = false;
                this.dialog = false;
            },
        },
    };
</script>