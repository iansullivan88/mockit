using System;

namespace Mockit.AspNetCore
{
    public sealed class HttpMock
    {
        public HttpMock(
            Guid id,
            HttpMockMatching matching,
            HttpMockResponse response) 
        {
            Id = id;
            Matching = matching;
            Response = response;
        }

        public Guid Id { get; }

        public HttpMockMatching Matching { get; }

        public HttpMockResponse Response { get; }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is HttpMock objMock))
            {
                return false;
            }

            return Id == objMock.Id;
        }

        public static bool operator ==(HttpMock left, HttpMock right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(HttpMock left, HttpMock right)
        {
            return !(left == right);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
