var btn = document.querySelectorAll(".item-delete")


btn.forEach(button => {
    button.addEventListener("click", function (e) {
        e.preventDefault()
        Swal.fire({
            title: "Eminmisin?",
            text: "Siliyimmi?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sil!"
        }).then((result) => {
            if (result.isConfirmed) {
                var url = button.getAttribute("href");
                fetch(url)
                    .then(resp => {
                        console.log(resp)
                        if (resp.status == 200) {
                            Swal.fire({
                                title: "Silindi!",
                                text: "Your file has been deleted.",
                                icon: "success"
                            });
                            button.parentElement.parentElement.remove()
                        }
                        else {
                            Swal.fire({
                                title: "Demisdim silme",
                                text: "Sile bilmedim",
                                icon: "error"
                            });
                        }

                    })
             
            }
        });
    })
})