(function () {
    "use strict";
    angular.module(AppName).component("faqManagerCategory", {
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        templateUrl: "/scripts/components/views/faqManagerCategoryModalView.html",
        controller: function (requestService, $scope, $window, $uibModal, $anchorScroll, $location) {
            var vm = this;
            vm.faqCategoryModel = {}; //this is the model that holds the info for the category form
            vm.faqCategoryList = []; //empty arry that gets cat list shoved in

            vm.$onInit = _init;
            vm.postFaqCategoryFromModal = _postFaqCategoryFromModal; //post category function
            vm.getAllFaqCategoriesFromModal = _getAllFaqCategoriesFromModal; //lists all category items
            vm.deleteFaqCategoryFromModal = _deleteFaqCategoryFromModal;
            vm.populateFaqCategoryBackToCategoryForm = _populateFaqCategoryBackToCategoryForm; //populating the category form so you can edit
            vm.closeFaqCategoryModalWindow = _closeFaqCategoryModalWindow;

            function _init() {
                _getAllFaqCategoriesFromModal();
            }

            function _postFaqCategoryFromModal(form) { //post or put depending if there is an id or not ==pass in the form and use the form.$ to make sure the form doesn't say "something soemtihng required"
                if (!vm.faqCategoryModel.id) {
                    requestService.ApiRequestService("post", "\/api/FaqCategories", vm.faqCategoryModel)
                        .then(function (response) {
                            swal({ title: "Done!", text: "FAQ category was successfully added", timer: 1800, showConfirmButton: false, type: "success" });
                            vm.faqCategoryModel = {};
                            form.$setPristine();
                            form.$setUntouched();
                            _getAllFaqCategoriesFromModal();
                        })
                        .catch(function (err) {
                            swal({ title: "Opps!", text: "Something went wrong: " + err.data, timer: 1800, showConfirmButton: false, type: "error" });

                            console.log(err);
                        })
                } else {
                    requestService.ApiRequestService("put", "\/api/FaqCategories/" + vm.faqCategoryModel.id, vm.faqCategoryModel)
                        .then(function (response) {
                            swal({ title: "Done!", text: "FAQ category was successfully edited", timer: 1800, showConfirmButton: false, type: "success" });
                            vm.faqCategoryModel = {};
                            form.$setPristine();
                            form.$setUntouched();
                            _getAllFaqCategoriesFromModal();
                        })
                        .catch(function (err) {
                            swal({ title: "Opps!", text: "Something went wrong: " + err.data, timer: 1800, showConfirmButton: false, type: "error" });

                        })
                }

            }

            function _getAllFaqCategoriesFromModal() {
                requestService.ApiRequestService("get", "\/api/FaqCategories")
                    .then(function (response) {
                        vm.faqCategoryList = [];
                        vm.faqCategoryList = response.items;
                    })
                    .catch(function (err) {
                        console.log(err);
                    })
            }

            function _deleteFaqCategoryFromModal(id) {
                swal({
                    title: "Are you sure you want to delete this FAQ category?",
                    text: "Note: All of the items in this category will be lost as well.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#FF0000",
                    confirmButtonText: "Okay",
                    closeOnCancel: true,
                    closeOnConfirm: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            requestService.ApiRequestService("delete", "\/api/FaqCategories/" + id)
                                .then(function (response) {
                                    vm.faqCategoryModel = {};
                                    vm.faqCategoryList = [];
                                    _getAllFaqCategoriesFromModal();

                                })
                                .catch(function (err) {
                                    swal({ title: "Opps!", text: "Something went wrong: " + err.data, timer: 1800, showConfirmButton: false, type: "error" });

                                    vm.faqCategoryModel = {};
                                });
                        }
                    });

            }

            function _populateFaqCategoryBackToCategoryForm(item) {
                vm.faqCategoryModel = item;
                $location.hash("faqCategoryFrm");
                $anchorScroll();
            }

            function _closeFaqCategoryModalWindow() {
                vm.close({ $value: "this is from modal window" });
            }
        }
    })
})();
