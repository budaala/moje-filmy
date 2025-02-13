import movieData from "../data.json";

// getting movies from database
const getMovies = async () => {
  return { success: true, data: movieData };
};

// getting movies from outside api and adding to database
const getNewMovies = async () => {
  return { success: true, data: movieData };
};

const addMovie = async (movie) => {
  console.log(movie);
  return { success: true, data: { id: 1 } };
};

const editMovie = async (movie) => {
  console.log(movie);
  return { success: true };
};

const deleteMovie = async (id) => {
  console.log("Deleting movie with id: " + id);
  return { success: true };
};

export default {
  getMovies,
  getNewMovies,
  addMovie,
  editMovie,
  deleteMovie,
};
