const getUserBtn = document.getElementById("getUserBtn");
const userContainer = document.getElementById("userContainer");
const saveUserBtn = document.getElementById("saveUserBtn");
const savedUsersList = document.getElementById("savedUsersList");
const darkModeBtn = document.getElementById("darkModeBtn");

let currentUser = null;
let savedUsers = [];

getUserBtn.addEventListener("click", async () => {
  const response = await fetch("https://randomuser.me/api/");
  const data = await response.json();
  const user = data.results[0];
  currentUser = {
    name: `${user.name.first} ${user.name.last}`,
    email: user.email,
    picture: user.picture.large
  };
  displayUser(currentUser);
});

function displayUser(user) {
  userContainer.innerHTML = `
    <img src="${user.picture}" alt="User Picture" />
    <p><strong>Name:</strong> ${user.name}</p>
    <p><strong>Email:</strong> ${user.email}</p>
  `;
}

saveUserBtn.addEventListener("click", () => {
  if (currentUser) {
    savedUsers.push(currentUser);
    updateSavedUsers();
  }
});

function updateSavedUsers() {
  savedUsersList.innerHTML = "";
  savedUsers.forEach((user, index) => {
    const li = document.createElement("li");
    li.innerHTML = `
      <button onclick="viewSavedUser(${index})">
        ${user.name}
      </button>
    `;
    savedUsersList.appendChild(li);
  });
}

window.viewSavedUser = function(index) {
  const user = savedUsers[index];
  displayUser(user);
};

darkModeBtn.addEventListener("click", () => {
  document.body.classList.toggle("dark-mode");
});
