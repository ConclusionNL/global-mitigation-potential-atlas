import { defineStore } from "pinia";
import { useLocalStorage } from "@vueuse/core";

export const useTutorialStore = defineStore("tutorial", {
  state: () => ({
    tut: false,
  }),
  getters: {
    getStatus() {
      return this.tut;
    },
  },
  actions: {
    setStatus(tutorial) {
      this.tut = tutorial;
    },
  },
});
