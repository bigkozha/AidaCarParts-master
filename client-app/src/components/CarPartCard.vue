<template>
  <v-card class="mx-auto" max-width="250" max-height="750">
    <v-img :src="picSrc" max-height="200px"></v-img>
    <v-card-title> {{ partName }} </v-card-title>
    <v-card-subtitle>
      Код товара: {{ partCode }}<br />
      OEM: {{ oem }}<br />
      {{ cost }}
    </v-card-subtitle>

    <v-card-actions>
      <v-btn color="orange lighten-2" text @click="show = !show"> Инфо </v-btn>

      <v-spacer></v-spacer>

      <v-btn icon @click="show = !show">
        <v-icon>{{ show ? "mdi-chevron-up" : "mdi-chevron-down" }}</v-icon>
      </v-btn>
    </v-card-actions>

    <div v-show="show">
      <v-divider></v-divider>
      <v-card-text>
        Применение: {{ note }}<br />
        Ед. измерения: {{ unitOfMeasure }}<br />
        Объем: {{ volume }}<br />
        Вес: {{ weight }}<br />
        Доступность: {{ availibility }}<br />
      </v-card-text>
      <v-btn color="orange lighten-2" text @click="editClicked" v-if="isAdmin"
        >Редактировать
      </v-btn>
    </div>
  </v-card>
</template>

<script>
export default {
  props: {
    id: Number,
    partName: String,
    note: String,
    unitOfMeasure: String,
    volume: String,
    weight: String,
    availibility: String,
    cost: String,
    oem: String,
    section: String,
    subsection: String,
    costNumber: String,
    picSrc: String,
    numerate: String,
    partCode: String,
    sectionAndSubsectionId: Number,
  },
  data: function () {
    return {
      show: false,
    };
  },
  methods: {
    editClicked() {
      this.$emit("editClicked", this.id);
    },
  },
  computed: {
    isAdmin() {
      return this.$store.getters.isLoggedIn;
    }
  }
};
</script>