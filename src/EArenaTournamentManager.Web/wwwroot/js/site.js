// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.bindImagePicker = function (fileInputId, hiddenInputId, previewId, category) {
	const fileInput = document.getElementById(fileInputId);
	const hiddenInput = document.getElementById(hiddenInputId);
	const preview = document.getElementById(previewId);

	if (!fileInput || !hiddenInput || !preview) {
		return;
	}

	hiddenInput.value = preview.getAttribute('src') || '';

	preview.addEventListener('click', () => fileInput.click());

	fileInput.addEventListener('change', async function () {
		const file = this.files && this.files[0];
		if (!file) {
			return;
		}

		const reader = new FileReader();
		reader.onload = e => {
			preview.src = e.target?.result || '';
			preview.classList.remove('d-none');
		};
		reader.readAsDataURL(file);

		const formData = new FormData();
		formData.append('file', file);

		try {
			const response = await fetch(`/Upload/Image?category=${encodeURIComponent(category)}`, {
				method: 'POST',
				body: formData
			});

			if (!response.ok) {
				alert('Image upload failed. Please try again.');
				return;
			}

			const data = await response.json();
			hiddenInput.value = data.url || '';

			if (data.url) {
				preview.src = data.url;
				preview.classList.remove('d-none');
			}
		} catch {
			alert('Image upload failed. Please try again.');
		}
	});
};
