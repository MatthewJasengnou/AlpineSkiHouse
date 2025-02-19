document.addEventListener('DOMContentLoaded', loadActivities);

async function loadActivities() {
    try {
        console.log("Fetching activities..."); // Debugging log
        const response = await fetch('/Index?handler=Activities');
        if (!response.ok) throw new Error(`Failed to fetch activities: ${response.statusText}`);
        const activities = await response.json();
        console.log("Activities received:", activities); // Debugging log

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

async function handleVote(activityId, isLike) {
    try {
        console.log(`Submitting vote: ActivityID=${activityId}, isLike=${isLike}`); // Debugging log
        const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]'); // Correctly selecting the CSRF token
        const headers = { 'Content-Type': 'application/json' };

        if (tokenElement && tokenElement.value) {
            headers['RequestVerificationToken'] = tokenElement.value;
        } else {
            console.error('CSRF token not found.');
            alert('Session might have expired or the page has an error. Please refresh and try again.');
            return;
        }

        const response = await fetch(`/Index?handler=Vote&activityId=${activityId}&isLike=${isLike}`, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({ activityId, isLike })
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
