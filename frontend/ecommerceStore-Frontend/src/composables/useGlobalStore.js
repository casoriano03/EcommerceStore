import { reactive } from "vue";

export const globalState = reactive({
    carts:[],
    itemCount:0,
    cartTotal:0,
});