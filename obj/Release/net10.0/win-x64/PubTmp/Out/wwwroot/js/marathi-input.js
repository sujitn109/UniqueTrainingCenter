// साध्या इंग्रजी शब्दांचे मराठीत रूपांतर करणारी स्क्रिप्ट (Transliteration)
function enableMarathi(id) {
    const el = document.getElementById(id);
    if (!el) return;

    el.addEventListener('input', function(e) {
        // हे एक साधे उदाहरण आहे, पूर्ण ट्रान्सलिटरेशनसाठी आपण लायब्ररी वापरतो
        // पण मोबाईलवर युजरला स्वतःचा कीबोर्ड वापरू देणे सर्वात सोपे पडते.
    });
}