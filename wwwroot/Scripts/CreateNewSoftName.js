$("#btnCreateNewSoftName").click(function () {
    $.ajax({
        url: "/Model/GetSelectListChildrenModels",
        method: "GET",
        success: function (data) {
            const models = data;
            const inputOptions = {};
            models.forEach((model) => {
                inputOptions[model.id] = model.name;
            });
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: "btn btn-success",
                    cancelButton: "btn btn-danger",
                },
                buttonsStyling: true,
            });
            swalWithBootstrapButtons
                .fire({
                    title: "Phần mềm mới thuộc Model",
                    input: "select",
                    inputOptions: inputOptions,
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Xác nhận",
                    cancelButtonText: "Huỷ bỏ",
                    reverseButtons: true,
                })
                .then((result) => {
                    if (result.isConfirmed) {
                        var idModel = result.value;
                        const swalWithBootstrapButtons = Swal.mixin({
                            customClass: {
                                confirmButton: "btn btn-success",
                                cancelButton: "btn btn-danger",
                            },
                            buttonsStyling: true,
                        });
                        swalWithBootstrapButtons
                            .fire({
                                title: "Nhập tên phần mềm",
                                input: "text",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonText: "Xác nhận thêm",
                                cancelButtonText: "Huỷ bỏ",
                                reverseButtons: true,
                            })
                            .then((result) => {
                                if (result.isConfirmed) {
                                    var nameSoft = result.value;
                                    //AJAX
                                    $.ajax({
                                        type: "POST",
                                        url: "/Soft/CreateSoftName",
                                        data: {
                                            name: nameSoft,
                                            modelId: idModel,
                                        },
                                        success: function (data) {
                                            if (!data.success) {
                                                console.log(
                                                    "Lỗi xảy ra: " +
                                                        data.message
                                                );
                                                Swal.fire({
                                                    icon: "error",
                                                    title: "Oops...",
                                                    text:
                                                        data.message +
                                                        " " +
                                                        nameSoft,
                                                });
                                            } else {
                                                Swal.fire(
                                                    "Thành công",
                                                    "Bạn vừa thêm phần mềm " +
                                                        nameSoft +
                                                        " thành công",
                                                    "success"
                                                ).then(function () {
                                                    window.location.reload();
                                                });
                                            }
                                        },
                                        error: function () {
                                            Swal.fire(
                                                "Lỗi máy chủ",
                                                "Thao tác đã được huỷ bỏ!",
                                                "error"
                                            );
                                        },
                                    });
                                } else if (
                                    result.dismiss === Swal.DismissReason.cancel
                                ) {
                                    swalWithBootstrapButtons.fire(
                                        "Đã huỷ",
                                        "Thao tác đã được huỷ bỏ!",
                                        "error"
                                    );
                                }
                            });
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        swalWithBootstrapButtons.fire(
                            "Đã huỷ",
                            "Thao tác đã được huỷ bỏ!",
                            "error"
                        );
                    }
                });
        },
        error: function () {
            Swal.fire("Lỗi máy chủ", "Thao tác đã được huỷ bỏ!", "error");
        },
    });
});
