using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MaxVonGrafKftMobileModel;
using MaxVonGrafKftMobileModel.AccessModels;
using Newtonsoft.Json;

namespace MaxVonGrafKftMobileServices.ApiService
{
    public class CustomerService
    {
        public CustomerReview getCustomerById(int customerId, string token)
        {
            CustomerReview customerReview = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "Customer/GetCustomer");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var url = string.Format(
                    client.BaseAddress +
                    "?customerId=" +
                    customerId);

                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        customerReview = JsonConvert.DeserializeObject<CustomerReview>(responseStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerReview;
        }

        public GetMobileCustomerByIDResponse getMobileCustomerById(GetMobileCustomerByIDRequest getMobileCustomerByIDRequest, string token)
        {
            GetMobileCustomerByIDResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/GetMobileCustomerByID");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(getMobileCustomerByIDRequest);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<GetMobileCustomerByIDResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int addInquiryChatModel(InquiryChatModel inquiryChatModel, string token)
        {
            int result = 0;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "Customer/AddInquiryChat");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(inquiryChatModel);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<int>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public GetCustomerPortalDetailsMobileResponse getCustomerDetailsWithProfilePic(GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest, string token)
        {

            GetCustomerPortalDetailsMobileResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/GetProfile");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(portalDetailsMobileRequest);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<GetCustomerPortalDetailsMobileResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<Inquiry> getInquries(int customerId, string token)
        {
            List<Inquiry> result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "Customer/GetAllInquiry");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var url = string.Format(
                    client.BaseAddress +
                    "?customerId=" +
                    customerId);

                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<List<Inquiry>>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<InquiryChatModel> getChatModels(int inquiryId, string token)
        {
            List<InquiryChatModel> result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "Customer/GetAllInquiryChat");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var url = string.Format(
                    client.BaseAddress +
                    "?inquiryId=" +
                    inquiryId);

                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<List<InquiryChatModel>>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int addInquiry(Inquiry newInquiry, string token)
        {
            int result = 0;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "Customer/AddInquiry");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(newInquiry);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<int>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public UpdateCustomerMobileCreditCardResponse UpdateMobileCustomerCreditCard(CreditCards cards, string token)
        {
            UpdateCustomerMobileCreditCardResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/UpdateMobileCustomerCreditCard");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(cards);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<UpdateCustomerMobileCreditCardResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DeleteCreditCardResponse deleteCreditCard(DeleteCreditCardRequest req, string token)
        {
            DeleteCreditCardResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/DeleteCreditCard");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(req);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<DeleteCreditCardResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public GetAllCustomerMobileCreditCardResponse GetAllMobileCustomerCreditCard(GetAllCustomerMobileCreditCardRequest cards, string token)
        {
            GetAllCustomerMobileCreditCardResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/GetAllMobileCustomerCreditCard");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(cards);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<GetAllCustomerMobileCreditCardResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public UpdateCustomerMobileCreditCardResponse AddMobileCustomerCreditCard(CustomerCreditCards customer, string token)
        {
            UpdateCustomerMobileCreditCardResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/AddMobileCustomerCreditCard");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(customer);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<UpdateCustomerMobileCreditCardResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public GetCustomerAgreementReservationCountResponse GetCustomerAgreementReservationCount(GetCustomerAgreementReservationCountRequest getCustomerAgreementReservationCountRequest, string token)
        {
            GetCustomerAgreementReservationCountResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "CustomerMobile/GetCustomerAgreementReservationCount");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(getCustomerAgreementReservationCountRequest);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<GetCustomerAgreementReservationCountResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<CustomerSeachResult> getCustomerByFilter(CustomerSerach customerSerach, string token)
        {
            List<CustomerSeachResult> result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "Customer/SearchAllCustomer");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(customerSerach);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<List<CustomerSeachResult>>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public GetForgetPasswordMobileResponse getForgetPasswordMobileRequest(GetForgetPasswordMobileRequest forgetPasswordMobileRequest, string token)
        {

            GetForgetPasswordMobileResponse result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConstantData.ApiURL.ToString() + "RegistrationMobile/ForgotPassword");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var myContent = JsonConvert.SerializeObject(forgetPasswordMobileRequest);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                    var response = client.PostAsync(client.BaseAddress, byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<GetForgetPasswordMobileResponse>(responseStream);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
