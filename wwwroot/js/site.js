document.addEventListener('DOMContentLoaded', loadActivities);

// Load activities from the server
async function loadActivities() {
    try {
        const response = await fetch('/Index?handler=Activities');
        if (!response.ok) throw new Error('Failed to fetch activities');
        const activities = await response.json();
        const container = document.getElementById('activities-container');
        const template = document.getElementById('activity-template').content;

        container.innerHTML = ''; // Clear previous contents

        activities.forEach(activity => {
            const clone = template.cloneNode(true);
            clone.querySelector('.card').setAttribute('data-id', activity.id);
            clone.querySelector('.card-title').textContent = activity.name;
            clone.querySelector('.card-text').textContent = activity.description;
            clone.querySelector('.like-btn .count').textContent = activity.likes;
            clone.querySelector('.dislike-btn .count').textContent = activity.dislikes;
            clone.querySelector('.like-btn').addEventListener('click', () => handleVote(activity.id, true));
            clone.querySelector('.dislike-btn').addEventListener('click', () => handleVote(activity.id, false));
            container.appendChild(clone);
        });
    } catch (error) {
        console.error('Error loading activities:', error);
        alert('Error loading activities. Please try again later.');
    }
}

// Handle voting
async function handleVote(activityId, isLike) {
    try {
        const response = await fetch(`/Index?handler=Vote&activityId=${activityId}&isLike=${isLike}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 
                       'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value },
        });

        if (!response.ok) throw new Error('Failed to submit vote');

        const updatedActivity = await response.json();
        const activityCard = document.querySelector(`.card[data-id="${activityId}"]`);
        activityCard.querySelector('.like-btn .count').textContent = updatedActivity.likes;
        activityCard.querySelector('.dislike-btn .count').textContent = updatedActivity.dislikes;

        // Optionally disable buttons after voting
        activityCard.querySelectorAll('button').forEach(btn => btn.disabled = true);
    } catch (error) {
        console.error('Error handling vote:', error);
        alert('Error submitting vote. Please try again later.');
    }
}
