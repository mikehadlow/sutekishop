using System;
using System.ServiceModel;
using System.Runtime.Serialization;
using CookComputing.XmlRpc;

// This code is taken from SubText http://sourceforge.net/projects/subtext

namespace Suteki.Shop.XmlRpc
{
    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct BlogInfo
    {
        [DataMember]
        public string blogid;

        [DataMember]
        public string url;

        [DataMember]
        public string blogName;
    }


    // TODO: following attribute is a temporary workaround
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct Enclosure
    {
        [DataMember]
        public int length;

        [DataMember]
        public string type;

        [DataMember]
        public string url;
    }

    // TODO: following attribute is a temporary workaround
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct Source
    {
        [DataMember]
        public string name;

        [DataMember]
        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        [DataMember]
        public DateTime dateCreated;

        [DataMember]
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string description;

        [DataMember]
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string title;

        [DataMember]
        [XmlRpcMember("categories", Description = "Contains categories for the post.")]
        public string[] categories;

        [DataMember]
        public Enclosure enclosure;

        [DataMember]
        public string link;

        [DataMember]
        public string permalink;

        [DataMember]
        [XmlRpcMember(
          Description = "Not required when posting. Depending on server may "
          + "be either string or integer. "
          + "Use Convert.ToInt32(postid) to treat as integer or "
          + "Convert.ToString(postid) to treat as string")]
        public object postid;

        [DataMember]
        public Source source;

        [DataMember]
        public string userid;
    }

    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct CategoryInfo
    {
        [DataMember]
        public string description;

        [DataMember]
        public string htmlUrl;

        [DataMember]
        public string rssUrl;

        [DataMember]
        public string title;

        [DataMember]
        public string categoryid;
    }

    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct Category
    {
        [DataMember]
        public string categoryId;

        [DataMember]
        public string categoryName;
    }

    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct mediaObject
    {
        [DataMember]
        public string name;

        [DataMember]
        public string type;

        [DataMember]
        public Byte[] bits;
    }

    [DataContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public struct mediaObjectInfo
    {
        [DataMember]
        public string url;
    }

    [ServiceContract(Namespace = "http://www.xmlrpc.com/metaWeblogApi")]
    public interface IMetaWeblog
    {
        [OperationContract(Action = "metaWeblog.editPost")]
        [XmlRpcMethod("metaWeblog.editPost", Description = "Updates and existing post to a designated blog "
          + "using the metaWeblog API. Returns true if completed.")]
        bool editPost(
         string postid,
         string username,
         string password,
         Post post,
         bool publish);

        [OperationContract(Action = "metaWeblog.getCategories")]
        [XmlRpcMethod("metaWeblog.getCategories",
          Description = "Retrieves a list of valid categories for a post "
          + "using the metaWeblog API. Returns the metaWeblog categories "
          + "struct collection.")]
        CategoryInfo[] getCategories(
          string blogid,
          string username,
          string password);

        [OperationContract(Action = "metaWeblog.getPost")]
        [XmlRpcMethod("metaWeblog.getPost",
          Description = "Retrieves an existing post using the metaWeblog "
          + "API. Returns the metaWeblog struct.")]
        Post getPost(
          string postid,
          string username,
          string password);

        [OperationContract(Action = "metaWeblog.getRecentPosts")]
        [XmlRpcMethod("metaWeblog.getRecentPosts",
          Description = "Retrieves a list of the most recent existing post "
          + "using the metaWeblog API. Returns the metaWeblog struct collection.")]
        Post[] getRecentPosts(
          string blogid,
          string username,
          string password,
          int numberOfPosts);

        [OperationContract(Action = "metaWeblog.newPost")]
        [XmlRpcMethod("metaWeblog.newPost",
          Description = "Makes a new post to a designated blog using the "
          + "metaWeblog API. Returns postid as a string.")]
        string newPost(
          string blogid,
          string username,
          string password,
          Post post,
          bool publish);

        [OperationContract(Action = "metaWeblog.newMediaObject")]
        [XmlRpcMethod("metaWeblog.newMediaObject",
          Description = "Uploads an image, movie, song, or other media "
          + "using the metaWeblog API. Returns the metaObject struct.")]
        mediaObjectInfo newMediaObject(object blogid, string username, string password, mediaObject mediaobject);

        #region BloggerAPI Members

        [OperationContract(Action = "blogger.deletePost")]
        [XmlRpcMethod("blogger.deletePost",
             Description = "Deletes a post.")]
        [return: XmlRpcReturnValue(Description = "Always returns true.")]
        bool deletePost(
            string appKey,
            string postid,
            string username,
            string password,
            [XmlRpcParameter(
                 Description = "Where applicable, this specifies whether the blog "
                 + "should be republished after the post has been deleted.")]
		  bool publish);

        [OperationContract(Action = "blogger.getUsersBlogs")]
        [XmlRpcMethod("blogger.getUsersBlogs",
             Description = "Returns information on all the blogs a given user "
             + "is a member.")]
        BlogInfo[] getUsersBlogs(
            string appKey,
            string username,
            string password);

        #endregion
    }
}
