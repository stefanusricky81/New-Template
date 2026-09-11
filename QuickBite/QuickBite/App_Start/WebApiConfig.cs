using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace QuickBite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            #region Login
            config.Routes.MapHttpRoute(
                name: "Login",
                routeTemplate: "api/Login/{useremail}/{password}",
                defaults: new { controller = "Login", action = "ValidateUser", useremail = RouteParameter.Optional, password = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "LoginForgotPassword",
                routeTemplate: "api/LoginForgotPassword/{username}",
                defaults: new { controller = "LoginForgotPassword", action = "ResetPassword", username = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "LoginForgotPassword",
            //    routeTemplate: "api/LoginForgotPassword/{username}/{oldpassword}/{newpassword}",
            //    defaults: new { controller = "LoginForgotPassword", action = "ResetPassword", username = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "LoginChangePassword",
            //    routeTemplate: "api/LoginChangePassword/{username}/{oldpassword}/{newpassword}",
            //    defaults: new { controller = "LoginChangePassword", action = "ChangeUserPassword", username = RouteParameter.Optional, oldpassword = RouteParameter.Optional, newpassword = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "LoginRegistration",
                routeTemplate: "api/LoginRegistration/{firstname}/{email}/{password}/{regtype}",
                defaults: new { controller = "LoginRegistration", action = "RegisterUser", firstname = RouteParameter.Optional, email = RouteParameter.Optional, password = RouteParameter.Optional, regtype = RouteParameter.Optional }
            );
            #endregion

            #region Notification Push
            config.Routes.MapHttpRoute(
                name: "NotificationPushAdd",
                routeTemplate: "api/NotificationPushAdd/{username}/{email}/{type}",
                defaults: new { controller = "NotificationPushAdd", action = "ChangeUsers", username = RouteParameter.Optional, email = RouteParameter.Optional, type = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "NotificationPush",
                routeTemplate: "api/NotificationPush/{message}",
                defaults: new { controller = "NotificationPush", action = "SendUsers", message = RouteParameter.Optional }
            );
            #endregion

            #region Movies

            config.Routes.MapHttpRoute(
                name: "MovieList",
                routeTemplate: "api/MovieList",
                defaults: new { controller = "MovieList", action = "GetAllMovie" }
            );

            config.Routes.MapHttpRoute(
                name: "MovieDetails",
                routeTemplate: "api/MovieDetails/{id}/{userid}",
                defaults: new { controller = "MovieDetails", action = "getMovies", Id = RouteParameter.Optional, userid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MovieDetailsComment",
                routeTemplate: "api/MovieDetailsComment/{userid}/{movieid}/{comments}",
                defaults: new { controller = "MovieDetailsComment", action = "PostComment", userid = RouteParameter.Optional, movieid = RouteParameter.Optional, comments = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MovieGetComment",
                routeTemplate: "api/MovieGetComment/{id}",
                defaults: new { controller = "MovieGetComment", action = "GetComment", id = RouteParameter.Optional }
            );

  
            config.Routes.MapHttpRoute(
                name: "MovieDetailsAddFav",
                routeTemplate: "api/MovieDetailsAddFav/{userid}/{FavId}/{FavType}",
                defaults: new { controller = "MovieDetailsAddFav", action = "AddFav", userid = RouteParameter.Optional, FavId = RouteParameter.Optional, FavType = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MovieDetailsAddWish",
                routeTemplate: "api/MovieDetailsAddWish/{userid}/{FavId}/{FavType}",
                defaults: new { controller = "MovieDetailsAddWish", action = "AddWish", userid = RouteParameter.Optional, FavId = RouteParameter.Optional, FavType = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MovieCastDetails",
                routeTemplate: "api/MovieCastDetails/{Castid}",
                defaults: new { controller = "MovieCastDetails", action = "GetCast", Castid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MovieRate",
                routeTemplate: "api/MovieRate/{movieid}/{userrate}/{userid}",
                defaults: new { controller = "MovieRate", action = "PostRate", movieid = RouteParameter.Optional, userrate = RouteParameter.Optional, userid = RouteParameter.Optional }
            );


            #endregion

            #region Profile
            config.Routes.MapHttpRoute(
                name: "Banner",
                routeTemplate: "api/ProfileBanner",
                defaults: new { controller = "ProfileBanner", action = "GetAllBanner" }
            );

            config.Routes.MapHttpRoute(
                name: "ProfileFavMovies",
                routeTemplate: "api/ProfileFavMovies/{emails}/{type}",
                defaults: new { controller = "ProfileFavMovies", action = "getFav", emails = RouteParameter.Optional, type = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ProfileWishMovies",
                routeTemplate: "api/ProfileWishMovies/{emails}/{type}",
                defaults: new { controller = "ProfileWishMovies", action = "getWish", emails = RouteParameter.Optional, type = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ProfileEditName",
                routeTemplate: "api/ProfileEditName/{userid}/{name}",
                defaults: new { controller = "ProfileEditName", action = "SetName", userid = RouteParameter.Optional, name = RouteParameter.Optional }
            );

            #endregion

            #region Search
            config.Routes.MapHttpRoute(
                name: "SearchAll",
                routeTemplate: "api/SearchAll/{MovTitle}/{Year}/{Genre}",
                defaults: new { controller = "SearchAll", action = "getAll", MovTitle = RouteParameter.Optional, Year = RouteParameter.Optional, Genre = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "SearchMovie",
                routeTemplate: "api/SearchMovie/{id}/{type}/{searchname}",
                defaults: new { controller = "SearchMovie", action = "getAll", id = RouteParameter.Optional, type = RouteParameter.Optional, searchname = RouteParameter.Optional }
            );
            #endregion


            //#region WishList
            //config.Routes.MapHttpRoute(
            //    name: "ProfileWishMovies",
            //    routeTemplate: "api/ProfileWishMovies/{emails}/{type}",
            //    defaults: new { controller = "ProfileWishMovies", action = "getFav", emails = RouteParameter.Optional, type = RouteParameter.Optional }
            //);
            //#endregion
            config.Formatters.Clear();
            //config.Formatters.Add(new XmlMediaTypeFormatter());
            config.Formatters.Add(new JsonMediaTypeFormatter());

        }
    }
}
