function getPassword(type) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: "/Home/GetPwd",
            data: { type: type },
            success: function (response) {
                if (response.success) {
                    resolve(response.data); // Trả về giá trị thành công
                } else {
                    reject(response.message); // Trả về thông báo lỗi
                }
            },
            error: function () {
                reject("Đã xảy ra lỗi khi gọi API.");
            },
        });
    });
}

$("#btnRootfolder").click(function () {
    getPassword("edit").then(function (password) {
        console.log("Mật khẩu: " + password);
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger",
            },
            buttonsStyling: true,
        });
        swalWithBootstrapButtons
            .fire({
                title: "Nhập mật khẩu Admin ",
                input: "password",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Xác nhận",
                cancelButtonText: "Huỷ bỏ",
                reverseButtons: true,
            })
            .then((result) => {
                if (result.isConfirmed) {
                    var matkhau = result.value;
                    if (matkhau == password) {
                        $.ajax({
                            type: "GET",
                            url: "/Home/OpenFolderInWWWRoot",
                            success: function () {
                                console.log("ok");
                            },
                            error: function () {
                                console.log("Đã xảy ra lỗi khi gọi API.");
                            },
                        });
                    } else {
                        swalWithBootstrapButtons.fire(
                            "Sai mật khẩu",
                            "Thao tác đã được huỷ bỏ!",
                            "error"
                        );
                    }
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    swalWithBootstrapButtons.fire(
                        "Đã huỷ",
                        "Thao tác đã được huỷ bỏ!",
                        "error"
                    );
                }
            });
    });
});
