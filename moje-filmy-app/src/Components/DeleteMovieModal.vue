<script setup>
import { ref, onMounted, onUnmounted } from "vue";
import { Modal } from "bootstrap";
import { eventBus } from "../eventBus";
import MovieService from "../services/MovieService";

const deleteModalRef = ref(null);
let modalInstance = null;

const movieId = ref(null);
const movieTitle = ref(null);

const errorMessage = ref(null);

onMounted(() => {
    if (deleteModalRef.value) {
        modalInstance = new Modal(deleteModalRef.value);
    }
    eventBus.on("openDeleteMovieModal", (data) => {
        movieId.value = data.movieId;
        movieTitle.value = data.movieTitle;
        modalInstance?.show();
    });
});

onUnmounted(() => {
    eventBus.off("openDeleteMovieModal");
});

const closeModal = () => {
    errorMessage.value = null;
    modalInstance?.hide();
};

const deleteMovie = async () => {
    var response = await MovieService.deleteMovie(movieId.value);
    if (response.success === true) {
        errorMessage.value = null;
        modalInstance?.hide();
        eventBus.emit("movieDeleted", movieId.value);
    }
    else {
        errorMessage.value = response.errorMessage;
        console.log(response);
    }
};
</script>

<template>
    <div class="modal fade" ref="deleteModalRef" id="deleteModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="deleteModalLabel">Potwierdź operację</h1>
                    <button type="button" class="btn-close" @click="closeModal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    Czy na pewno chcesz usunąć film <b>{{ movieTitle }}</b>?
                </div>
                <div v-if="errorMessage" class="alert alert-danger error-alert" role="alert">
                    <p>Wystąpił błąd podczas usuwania filmu: </p>
                    {{ errorMessage }}
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="closeModal">Anuluj</button>
                    <button type="button" class="btn btn-danger" @click="deleteMovie">Usuń film</button>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.error-alert {
    margin: 10px;
}

</style>