function submitForm() {
    var description = document.getElementById('description').value;

    var data = {
        Description: description
    };

    fetch('/DeepAI/GenerateImage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': '@Html.AntiForgeryToken()'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            console.log('Image URL:', data.imageUrl);
        })
        .catch(error => console.error('Error:', error));
}
